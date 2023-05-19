/* eslint-disable @typescript-eslint/no-this-alias */
/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable no-restricted-syntax */
/* eslint-disable eqeqeq */
/* eslint-disable no-underscore-dangle */
/* eslint-disable no-param-reassign */
/* eslint-disable no-plusplus */
import moduleHelper from './module-helper';
import { uid } from './utils';
import {
  isAndroid, webAudioNeedResume, isSupportBufferURL, isSupportPlayBackRate, isSupportCacheAudio, isPc,
} from '../check-version';

// UnityAudio对象池
const WEBAudio = {
  audioInstanceIdCounter: 0,
  audioInstances: {},
  audioContext: null,
  audioWebEnabled: 0,
  audioCache: [], // 缓存innerAudio
  lOrientation: {
    x: 0,
    y: 0,
    z: 0,
    xUp: 0,
    yUp: 0,
    zUp: 0,
  },
  lPosition: { x: 0, y: 0, z: 0 },
  audio3DSupport: 0, // 是否支持3d音效，默认不支持
  audioWebSupport: 0, // 判断客户端是否支持webAudio
};
// innerAudio对象池
const audios = {};
// webAudio对象使用个数
let bufferSourceNodeLength = 0;
// audio总共的大小
let audioBufferLength = 0;
// 错误提示
const msg = 'InnerAudioContext does not exist!';
const ignoreErrorMsg = 'audio is playing, don\'t play again';
// 当前生命周期内的临时音频路径
const localAudioMap = {};
// 正在下载中的音频
const downloadingAudioMap = {};
// 缓存音量设置
const soundVolumeHandler = {};
const err = (msg) => {
  GameGlobal.manager.printErr(msg);
};
// 重置音频本地缓存文件夹
function mkCacheDir() {
  const fs = wx.getFileSystemManager();
  fs.rmdir({
    dirPath: `${wx.env.USER_DATA_PATH}/__GAME_FILE_CACHE/audios`,
    recursive: true,
    complete: () => {
      fs.mkdir({
        dirPath: `${wx.env.USER_DATA_PATH}/__GAME_FILE_CACHE/audios`,
      });
    },
  });
}

mkCacheDir();

const WXGetAudioCount = () => ({
  innerAudio: Object.keys(audios).length,
  webAudio: bufferSourceNodeLength,
  buffer: audioBufferLength,
});

const funs = {
  // 获取完整路径
  getFullUrl(v) {
    if (!/^https?:\/\//.test(v) && !/^wxfile:\/\//.test(v)) {
      const cdnPath = GameGlobal.manager.assetPath;
      v = `${cdnPath.replace(/\/$/, '')}/${v.replace(/^\//, '').replace(/^Assets\//, '')}`;
    }
    return v;
  },
  // 下载并保存音频列表
  downloadAudios(paths) {
    const list = paths.split(',');
    return Promise.all(list.map((v) => {
      const src = funs.getFullUrl(v);
      // eslint-disable-next-line @typescript-eslint/no-misused-promises
      return new Promise(async (resolve, reject) => {
        // 是否不在下载中
        if (!downloadingAudioMap[src]) {
          downloadingAudioMap[src] = [
            {
              resolve,
              reject,
            },
          ];
          if (funs.checkLocalFile(src)) {
            funs.handleDownloadEnd(src, true);
          } else if (!GameGlobal.unityNamespace.isCacheableFile(src)) {
            // console.warn(`${src} 不在的缓存路径内，\n如需保存本地，请按照 https://github.com/wechat-miniprogram/minigame-unity-webgl-transform/blob/main/Design/FileCache.md 设置配置`);
            wx.downloadFile({
              url: src,
              success(res) {
                if (res.statusCode === 200 && res.tempFilePath) {
                  localAudioMap[src] = res.tempFilePath;
                  funs.handleDownloadEnd(src, true);
                } else {
                  funs.handleDownloadEnd(src, false);
                }
              },
              fail(e) {
                funs.handleDownloadEnd(src, false);
                err(e);
              },
            });
          } else {
            const xmlhttp = new GameGlobal.unityNamespace.UnityLoader.UnityCache.XMLHttpRequest();
            xmlhttp.open('GET', src, true);
            xmlhttp.responseType = 'arraybuffer';
            xmlhttp.onsave = () => {
              localAudioMap[src] = GameGlobal.manager.getCachePath(src);
              funs.handleDownloadEnd(src, true);
            };
            xmlhttp.onsavefail = () => {
              funs.handleDownloadEnd(src, false);
            };
            xmlhttp.onerror = () => {
              funs.handleDownloadEnd(src, false);
            };
            xmlhttp.send();
          }
        } else {
          downloadingAudioMap[src].push({
            resolve,
            reject,
          });
        }
      });
    }));
  },
  // 下载完成回调
  handleDownloadEnd(src, succeeded) {
    if (!downloadingAudioMap[src]) {
      return;
    }
    while (downloadingAudioMap[src] && downloadingAudioMap[src].length > 0) {
      const item = downloadingAudioMap[src].shift();
      if (!succeeded) {
        item.reject();
      } else {
        item.resolve();
      }
    }
    downloadingAudioMap[src] = null;
  },
  // 是否存在本地文件
  checkLocalFile(src) {
    if (localAudioMap[src]) {
      return true;
    }
    const path = GameGlobal.manager.getCachePath(src);
    if (path) {
      localAudioMap[src] = path;
      return true;
    }
    return false;
  },
  // 设置路径
  setAudioSrc(audio, getSrc) {
    return new Promise((resolve, reject) => {
      const src = funs.getFullUrl(getSrc);
      // 设置原始路径，后面用此路径作为key值
      audio.isLoading = src;
      if (funs.checkLocalFile(src)) {
        audio.src = localAudioMap[src];
        audio.isLoading = false;
        funs.handleDownloadEnd(src, true);
        resolve(localAudioMap[src]);
      } else if (audio._needDownload) {
        funs
          .downloadAudios(src)
          .then(() => {
            if (audio) {
              audio.src = localAudioMap[src];
              audio.isLoading = false;
              resolve(localAudioMap[src]);
            } else {
              console.warn('音频已被删除:', src);
              reject();
            }
          })
          .catch(() => {
            console.warn('资源下载失败:', src);
            if (audio) {
              audio.src = src;
              audio.isLoading = false;
            }
            reject();
          });
      } else {
        // 不推荐这样处理，建议优先下载再使用，除非是需要立即播放的长音频文件或一次性播放音频
        // console.warn('建议优先下载再使用:', src);
        audio.src = src;
        audio.isLoading = false;
        resolve(src);
      }
    });
  },
};

const resumeWebAudio = () => {
  if (
    WEBAudio.audioContext
    && (WEBAudio.audioContext.state === 'suspended' || WEBAudio.audioContext.state === 'interrupted')
  ) {
    WEBAudio.audioContext.resume();
  }
};

const createInnerAudio = () => {
  const id = uid();
  const audio = isSupportCacheAudio && WEBAudio.audioCache.length
    ? WEBAudio.audioCache.pop()
    : wx.createInnerAudioContext();
  audios[id] = audio;
  return {
    id,
    audio,
  };
};

const destroyInnerAudio = (id, useCache) => {
  if (!useCache || !isSupportCacheAudio || WEBAudio.audioCache.length > 32) {
    audios[id].destroy();
  } else {
    // 重置innerAudio，复用对象
    audios[id].stop();
    ['Play', 'Pause', 'Stop', 'Canplay', 'Error', 'Ended', 'Waiting', 'Seeking', 'Seeked', 'TimeUpdate'].forEach((eventName) => {
      audios[id][`off${eventName}`]();
    });
    const state = {
      startTime: 0,
      obeyMuteSwitch: true,
      volume: 1,
      autoplay: false,
      loop: false,
      referrerPolicy: '',
    };
    for (const key in state) {
      try {
        audios[id][key] = state[key];
      } catch (e) {}
    }
    // 放回缓存
    WEBAudio.audioCache.push(audios[id]);
  }
  delete audios[id];
};

export default {
  // 创建audio对象
  WXCreateInnerAudioContext(src, loop, startTime, autoplay, volume, playbackRate, needDownload) {
    const { audio: getAudio, id } = createInnerAudio();
    getAudio._needDownload = needDownload;
    if (src) {
      // 设置原始src
      funs.setAudioSrc(getAudio, src).catch(() => {
        moduleHelper.send(
          'OnAudioCallback',
          JSON.stringify({
            callbackId: id,
            errMsg: 'onError',
          }),
        );
      });
    }
    if (loop) {
      getAudio.loop = true;
    }
    if (autoplay) {
      getAudio.autoplay = true;
    }
    if (typeof startTime === 'undefined') {
      startTime = 0;
    }
    if (startTime > 0) {
      getAudio.startTime = +startTime.toFixed(2);
    }
    if (typeof volume === 'undefined') {
      volume = 1;
    }
    if (volume !== 1) {
      getAudio.volume = +volume.toFixed(2);
    }
    // 低版本安卓有bug，不支持playbackRate
    if (!isSupportPlayBackRate) {
      playbackRate = 1;
    }
    if (playbackRate !== 1) {
      getAudio.playbackRate = +playbackRate.toFixed(2);
    }
    return id;
  },
  WXInnerAudioContextSetBool(id, k, v) {
    if (audios[id]) {
      audios[id][k] = Boolean(+v);
    } else {
      console.error(msg, id);
    }
  },
  // 修改属性
  WXInnerAudioContextSetString(id, k, v) {
    if (audios[id]) {
      // 如果修改的是src，则需要做特殊处理，如果之前设定了这个audio needDownload，则触发下载
      if (k === 'src') {
        funs.setAudioSrc(audios[id], v);
      } else if (k === 'needDownload') {
        audios[id]._needDownload = !!v;
      } else {
        audios[id][k] = v;
      }
    } else {
      console.error(msg, id);
    }
  },
  WXInnerAudioContextSetFloat(id, k, v) {
    if (audios[id]) {
      audios[id][k] = +v.toFixed(2);
    } else {
      console.error(msg, id);
    }
  },
  WXInnerAudioContextGetFloat(id, k) {
    if (audios[id]) {
      return audios[id][k];
    }
    console.error(msg, id);
    return 0;
  },
  WXInnerAudioContextGetBool(id, k) {
    if (audios[id]) {
      return audios[id][k];
    }
    console.error(msg, id);
    return false;
  },
  WXInnerAudioContextPlay(id) {
    if (audios[id]) {
      if (audios[id].isLoading) {
        if (downloadingAudioMap[audios[id].isLoading]) {
          downloadingAudioMap[audios[id].isLoading].push({
            resolve: () => {
              if (typeof audios[id] !== 'undefined') {
                audios[id].play();
              }
            },
            reject: () => {},
          });
        } else {
          audios[id].src = audios[id].isLoading;
          audios[id].play();
        }
      } else {
        audios[id].play();
      }
    } else {
      console.error(msg, id);
    }
  },
  WXInnerAudioContextPause(id) {
    if (audios[id]) {
      audios[id].pause();
    } else {
      console.error(msg, id);
    }
  },
  WXInnerAudioContextStop(id) {
    if (audios[id]) {
      audios[id].stop();
    } else {
      console.error(msg, id);
    }
  },
  WXInnerAudioContextDestroy(id) {
    if (audios[id]) {
      destroyInnerAudio(id, false);
    } else {
      console.error(msg, id);
    }
  },
  WXInnerAudioContextSeek(id, position) {
    if (audios[id]) {
      audios[id].seek(+position.toFixed(3));
    } else {
      console.error(msg, id);
    }
  },
  // 监听事件
  WXInnerAudioContextAddListener(id, key) {
    if (audios[id]) {
      const AddListener = () => {
        if (!audios[id]) {
          return;
        }
        if (key === 'onCanplay') {
          audios[id][key](() => {
            // 兼容基础库获取属性异常的bug
            const {
              duration, buffered, referrerPolicy, volume,
            } = audios[id];
            setTimeout(() => {
              moduleHelper.send(
                'OnAudioCallback',
                JSON.stringify({
                  callbackId: id,
                  errMsg: key,
                }),
              );
            }, 0);
          });
        } else {
          audios[id][key]((e) => {
            if (key === 'onError') {
              console.warn('innerAudio onError:');
              err(e);
              // 忽略安卓一些告警报错
              if (e.errMsg && e.errMsg.indexOf(ignoreErrorMsg) > -1) {
                return;
              }
            }
            moduleHelper.send(
              'OnAudioCallback',
              JSON.stringify({
                callbackId: id,
                errMsg: key,
              }),
            );
          });
        }
      };

      // 兼容innerAudio已初始化，但音频还在下载中，先等待音频下载完毕
      // if (!audios[id].src && audios[id]._needDownload && audios[id]._src) {
      //   const src = audios[id]._src;
      //   if (!downloadingAudioMap[src]) {
      //     downloadingAudioMap[src] = [];
      //   }
      //   downloadingAudioMap[src].push({
      //     resolve: AddListener,
      //     reject: () => {},
      //   });
      // } else {
      //   AddListener();
      // }
      AddListener();
    } else {
      console.error(msg, id);
    }
  },
  WXInnerAudioContextRemoveListener(id, key) {
    if (audios[id]) {
      audios[id][key]();
    } else {
      console.error(msg, id);
    }
  },
  WXPreDownloadAudios(paths, id) {
    funs
      .downloadAudios(paths)
      .then(() => {
        moduleHelper.send(
          'WXPreDownloadAudiosCallback',
          JSON.stringify({
            callbackId: id.toString(),
            errMsg: '0',
          }),
        );
      })
      .catch(() => {
        moduleHelper.send(
          'WXPreDownloadAudiosCallback',
          JSON.stringify({
            callbackId: id.toString(),
            errMsg: '1',
          }),
        );
      });
  },
  WXGetAudioCount,
  // -------------------Unity Audio适配--------------------
  _JS_Sound_Create_Channel(callback, userData) {
    if (WEBAudio.audioWebEnabled === 0) {
      return;
    }
    const channel = {
      gain: WEBAudio.audioContext && WEBAudio.audioContext.createGain(),
      threeD: false,
      release() {
        this.disconnectSource();
        if (this.gain) {
          this.gain.disconnect();
        }
      },
      playUrl(startTime, url, startOffset, volume, soundClip) {
        try {
          if (this.source && url === this.source.url) {
            this.source.start(startTime, startOffset);
            return;
          }
          this.setup(url);
          if (typeof volume !== 'undefined') {
            this.source.mediaElement.volume = volume;
          }
          const chan = this;
          this.source.mediaElement.onPlay(() => {
            if (typeof this.source !== 'undefined') {
              this.source.isPlaying = true;
              if (!this.source.loop && this.source.mediaElement) {
                const { duration } = this.source.mediaElement;
                if (duration) {
                  if (this.source.stopTicker) {
                    clearTimeout(this.source.stopTicker);
                    this.source.stopTicker = 0;
                  }
                  const time = Math.floor(duration * 1000) + 1000;
                  this.source.stopTicker = setTimeout(() => {
                    if (this.source && this.source.mediaElement) {
                      this.source.mediaElement.stop();
                    }
                  }, time);
                }
              }
            }
          });
          this.source.mediaElement.onPause(() => {
            if (typeof this.source !== 'undefined') {
              this.source.isPlaying = false;
              if (this.source.stopTicker) {
                clearTimeout(this.source.stopTicker);
                this.source.stopTicker = 0;
              }
            }
          });
          this.source.mediaElement.onStop(() => {
            if (typeof this.source !== 'undefined') {
              if (this.source.playAfterStop) {
                this.source._reset();
                if (typeof this.source.mediaElement !== 'undefined') {
                  this.source.mediaElement.play();
                }
                return;
              }
              this.source._reset();
              chan.disconnectSource();
            }
            if (callback) {
              GameGlobal.unityNamespace.Module.dynCall_vi(callback, [userData]);
            }
          });
          this.source.mediaElement.onEnded(() => {
            if (typeof this.source !== 'undefined') {
              this.source._reset();
              chan.disconnectSource();
            }
            if (callback) {
              GameGlobal.unityNamespace.Module.dynCall_vi(callback, [userData]);
            }
          });
          this.source.mediaElement.onError((e) => {
            console.warn('innerAudio onError:');
            err(e);
            const { errMsg } = e;
            if (!errMsg) {
              return;
            }
            // 忽略安卓一些告警报错
            if (errMsg.indexOf(ignoreErrorMsg) > -1) {
              return;
            }
            // 播放失败
            if (errMsg.indexOf('play audio fail') > -1 && typeof this.source !== 'undefined' && this.source.mediaElement) {
              this.source._reset();
              this.source.mediaElement.stop();
            }
          });
          this.source.mediaElement.onCanplay(() => {
            if (typeof this.source !== 'undefined' && this.source.mediaElement) {
              const { duration } = this.source.mediaElement;
              setTimeout(() => {
                if (soundClip && this.source && this.source.mediaElement) {
                  soundClip.length = Math.round(this.source.mediaElement.duration * 44100);
                }
              }, 0);
            }
          });
          this.source.start(startTime, startOffset);
          this.source.playbackStartTime = startTime - startOffset / this.source.playbackRateValue;
        } catch (e) {
          err(`playUrl error. Exception: ${e}`);
        }
      },
      playBuffer(startTime, buffer, startOffset) {
        try {
          this.setup();
          this.source.buffer = buffer;
          const chan = this;
          this.source.onended = function () {
            chan.disconnectSource();
            if (callback) {
              GameGlobal.unityNamespace.Module.dynCall_vi(callback, [userData]);
            }
          };
          this.source.start(startTime, startOffset);
          this.source.playbackStartTime = startTime - startOffset / this.source.playbackRateValue;
        } catch (e) {
          err(`playBuffer error. Exception: ${e}`);
        }
      },
      disconnectSource() {
        if (this.source) {
          if (this.source.mediaElement) {
            destroyInnerAudio(this.source.instanceId, true);
            delete this.source.mediaElement;
            delete this.source;
          } else if (!this.source.isPausedMockNode) {
            this.source.onended = null;
            if (this.source.disconnect) {
              this.source.disconnect();
            }
            if (GameGlobal.isIOSHighPerformanceMode) {
              this.source.buffer = null;
            }
            bufferSourceNodeLength -= 1;
            delete this.source;
          } else {
            this.source.buffer = null;
          }
        }
      },
      stop(delay) {
        if (this.source) {
          if (this.source.buffer) {
            try {
              this.source.stop(WEBAudio.audioContext.currentTime + delay);
            } catch (e) {}
            if (delay == 0) {
              this.disconnectSource();
            }
          } else if (this.source.mediaElement) {
            this.source.stop(delay);
          }
        }
      },
      isPaused() {
        if (!this.source) {
          return true;
        }
        if (this.source.isPausedMockNode) {
          return true;
        }
        if (this.source.mediaElement) {
          return this.source.mediaElement.paused || this.source.pauseRequested;
        }
        return false;
      },
      pause() {
        const s = this.source;
        if (!s) {
          return;
        }
        if (s.mediaElement) {
          s._pauseMediaElement();
          return;
        }
        if (s.isPausedMockNode) {
          return;
        }
        const pausedSource = {
          isPausedMockNode: true,
          loop: s.loop,
          loopStart: s.loopStart,
          loopEnd: s.loopEnd,
          buffer: s.buffer,
          playbackRate: s.playbackRateValue,
          playbackPausedAtPosition: s.estimatePlaybackPosition(),
          setPitch(v) {
            this.playbackRate = v;
          },
          _reset() {},
        };
        this.stop(0);
        this.disconnectSource();
        this.source = pausedSource;
      },
      resume() {
        const pausedSource = this.source;
        if (!pausedSource) {
          return;
        }
        if (pausedSource.mediaElement) {
          pausedSource.start();
          return;
        }
        if (!pausedSource.isPausedMockNode) {
          return;
        }
        delete this.source;
        if (!pausedSource.buffer) {
          return;
        }
        this.playBuffer(
          WEBAudio.audioContext.currentTime - Math.min(0, pausedSource.playbackPausedAtPosition),
          pausedSource.buffer,
          Math.max(0, pausedSource.playbackPausedAtPosition),
        );
        this.source.loop = pausedSource.loop;
        this.source.loopStart = pausedSource.loopStart;
        this.source.loopEnd = pausedSource.loopEnd;
        this.source.setPitch(pausedSource.playbackRate);
      },
      setVolume(volume) {
        if (this.source) {
          if (this.source.buffer && this.gain) {
            this.gain.gain.setValueAtTime(volume, WEBAudio.audioContext.currentTime);
          } else if (this.source.mediaElement) {
            this.source.mediaElement.volume = volume;
          }
        }
      },
      setup(url) {
        if (this.source && !this.source.isPausedMockNode) {
          if (!this.source.url) {
            if (typeof url !== 'undefined') {
              // 从webAudio切换到innerAudio
              this.stop(0);
            } else {
              // 从webAudio切换到webAudio不做特殊处理
            }
          } else if (typeof url === 'undefined') {
            // 从innerAudio切换到webAudio
            this.source._reset();
            this.disconnectSource();
          } else {
            if (this.source.url === url) {
              // 从innerAudio切换到innerAudio
              // 播放同一个实例
              return;
            }
            if (url !== this.source.url) {
              // 从innerAudio切换到innerAudio
              // 客户端有bug尚未修复，复用时无法触发onCanplay，所以此处都先销毁
              this.source._reset();
              this.disconnectSource();
              //   this.source.mediaElement.src = url
              //   this.source.url = url
              //   return
            }
          }
        }
        if (!url) {
          this.source = WEBAudio.audioContext.createBufferSource();
          bufferSourceNodeLength += 1;
          const chan = this;
          Object.defineProperty(this.source, 'playbackRateValue', {
            get() {
              return chan.source.playbackRate.value;
            },
            set(v) {
              chan.source.playbackRate.value = v;
            },
          });
        } else {
          const { audio: getAudio, id: instanceId } = createInnerAudio();
          getAudio.src = url;
          this.source = {
            instanceId,
            mediaElement: getAudio,
            url,
            playbackStartTime: 0,
          };
          const { buffered, referrerPolicy, volume } = getAudio;
          const { source } = this;
          Object.defineProperty(this.source, 'loopStart', {
            get() {
              return 0;
            },
            set(v) {},
          });
          Object.defineProperty(source, 'loopEnd', {
            get() {
              return 0;
            },
            set(v) {},
          });
          Object.defineProperty(source, 'loop', {
            get() {
              return source.mediaElement.loop;
            },
            set(v) {
              source.mediaElement.loop = v;
            },
          });
          Object.defineProperty(source, 'playbackRateValue', {
            get() {
              return source.mediaElement.playbackRate;
            },
            set(v) {
              // 低版本安卓有bug，不支持playbackRate
              if (!isSupportPlayBackRate) {
                source.mediaElement.playbackRate = 1;
              } else {
                source.mediaElement.playbackRate = v;
              }
            },
          });
          Object.defineProperty(source, 'currentTime', {
            get() {
              return source.mediaElement.currentTime;
            },
            set(v) {
              if (typeof source.mediaElement.seek === 'function') {
                source.mediaElement.seek(v);
              } else {
                source.mediaElement.currentTime = v;
              }
            },
          });
          // Object.defineProperty(this.source, 'mute', {
          //   get() {
          //     return source.mediaElement.mute;
          //   },
          //   set(v) {
          //     source.mediaElement.mute = v;
          //   },
          // });
          const innerFixPlay = () => {
            if (!this.source) {
              return;
            }
            this.source.needCanPlay = true;
            if (this.source.fixPlayTicker) {
              // 防止安卓重复触发导致error
              clearTimeout(this.source.fixPlayTicker);
            }
            // 兜底，客户端有概率不会触发onCanplay或者没有触发onPlay
            this.source.fixPlayTicker = setTimeout(() => {
              if (this.source && this.source.mediaElement && this.source.needCanPlay && !this.source.isPlaying) {
                this.source.mediaElement.play();
              }
            }, 2000);
          };
          const innerPlay = () => {
            if (this.source && this.source.mediaElement) {
              if (isSupportBufferURL && this.source.readyToPlay) {
                if (this.source.stopCache) {
                  this.source.stopCache = false;
                  this.source.playAfterStop = true;
                } else if (!this.source.isPlaying) {
                  // 安卓有一定概率调用play无任何反应
                  if (isAndroid) {
                    innerFixPlay();
                  }
                  this.source.mediaElement.play();
                }
              } else {
                this.source.mediaElement.onCanplay(() => {
                  if (!this.source) {
                    return;
                  }
                  this.source.needCanPlay = false;
                  this.source.readyToPlay = true;
                  if (typeof this.source.mediaElement !== 'undefined') {
                    // duration兼容用
                    const { duration } = this.source.mediaElement;
                    this.source.mediaElement.offCanplay();
                  }
                  if (this.source.stopCache) {
                    this.source.stopCache = false;
                    this.source.playAfterStop = true;
                  } else if (!this.source.isPlaying) {
                    // 安卓有一定概率调用play无任何反应
                    if (isAndroid) {
                      innerFixPlay();
                    }
                    if (typeof this.source.mediaElement !== 'undefined') {
                      this.source.mediaElement.play();
                    }
                  }
                });
                innerFixPlay();
              }
            }
          };
          this.source._reset = () => {
            if (!this.source) {
              return;
            }
            this.source.readyToPlay = false;
            this.source.isPlaying = false;
            this.source.stopCache = false;
            this.source.playAfterStop = false;
            this.source.needCanPlay = false;
            if (this.source.stopTicker) {
              clearTimeout(this.source.stopTicker);
              this.source.stopTicker = 0;
            }
          };
          this.source.playTimeout = null;
          this.source.pauseRequested = false;
          this.source._pauseMediaElement = () => {
            if (typeof this.source === 'undefined') {
              return;
            }
            if (this.source.playTimeout) {
              this.source.pauseRequested = true;
            } else if (this.source.isPlaying && this.source.mediaElement) {
              this.source.mediaElement.pause();
            }
          };
          this.source._startPlayback = (offset) => {
            if (typeof this.source === 'undefined') {
              return;
            }
            if (this.source.playTimeout) {
              if (typeof this.source.mediaElement.seek === 'function') {
                this.source.mediaElement.seek(offset);
              } else {
                this.source.mediaElement.currentTime = offset;
              }
              this.source.pauseRequested = false;
              return;
            }
            innerPlay();
            if (typeof source.mediaElement.seek === 'function') {
              this.source.mediaElement.seek(offset);
            } else {
              this.source.mediaElement.currentTime = offset;
            }
          };
          this.source.start = (startTime, offset) => {
            if (typeof this.source === 'undefined') {
              return;
            }
            if (typeof startTime === 'undefined' && typeof offset === 'undefined') {
              innerPlay();
              return;
            }
            if (typeof startTime === 'undefined') {
              startTime = 0;
            }
            if (typeof offset === 'undefined') {
              offset = 0;
            }
            const startDelayThresholdMS = 4;
            const startDelayMS = startTime * 1e3;
            if (startDelayMS > startDelayThresholdMS) {
              if (this.source.playTimeout) {
                clearTimeout(this.source.playTimeout);
              }
              this.source.playTimeout = setTimeout(() => {
                if (typeof this.source !== 'undefined') {
                  this.source.playTimeout = null;
                  this.source._startPlayback(offset);
                }
              }, startDelayMS);
            } else {
              this.source._startPlayback(offset);
            }
          };
          this.source.stop = (stopTime) => {
            if (typeof this.source === 'undefined') {
              return;
            }
            if (typeof stopTime === 'undefined') {
              stopTime = 0;
            }
            const stopDelayThresholdMS = 4;
            const stopDelayMS = stopTime * 1e3;
            if (stopDelayMS > stopDelayThresholdMS) {
              setTimeout(() => {
                if (this.source && this.source.mediaElement) {
                  this.source.stopCache = true;
                  this.source.mediaElement.stop();
                }
              }, stopDelayMS);
            } else if (this.source.mediaElement) {
              this.source.stopCache = true;
              this.source.mediaElement.stop();
            }
          };
        }
        this.source.estimatePlaybackPosition = function () {
          let t;
          if (WEBAudio.audioContext) {
            t = (WEBAudio.audioContext.currentTime - this.playbackStartTime) * this.playbackRateValue;
          } else {
            t = -this.playbackStartTime * this.playbackRateValue;
          }
          if (this.loop && t >= this.loopStart) {
            t = ((t - this.loopStart) % (this.loopEnd - this.loopStart)) + this.loopStart;
          }
          return t;
        };
        const { source } = this;
        this.source.setPitch = function (newPitch) {
          const curPosition = source.estimatePlaybackPosition();
          if (curPosition >= 0) {
            if (WEBAudio.audioContext) {
              this.playbackStartTime = WEBAudio.audioContext.currentTime - curPosition / newPitch;
            }
          }
          this.playbackRateValue = newPitch;
        };
        this.setupPanning();
      },
      setupPanning() {
        if (typeof this.source === 'undefined') {
          return;
        }
        if (this.source.isPausedMockNode) {
          return;
        }
        if (this.source.disconnect && this.source.connect) {
          this.source.disconnect();

          this.source.connect(this.gain);
        }
      },
      isStopped() {
        return !this.source;
      },
    };
    if (channel.gain) {
      channel.gain.connect(WEBAudio.audioContext.destination);
    }
    WEBAudio.audioInstances[++WEBAudio.audioInstanceIdCounter] = channel;
    return WEBAudio.audioInstanceIdCounter;
  },
  _JS_Sound_GetLength(bufferInstance) {
    if (WEBAudio.audioWebEnabled === 0) {
      return 441000;
    }
    const soundClip = WEBAudio.audioInstances[bufferInstance];
    if (!soundClip) {
      return 441000;
    }
    if (soundClip && soundClip.buffer) {
      const sampleRateRatio = 44100 / soundClip.buffer.sampleRate;
      return soundClip.buffer.length * sampleRateRatio;
    }
    return soundClip.length;
  },
  _JS_Sound_GetLoadState(bufferInstance) {
    if (WEBAudio.audioWebEnabled === 0) {
      return 2;
    }
    const soundClip = WEBAudio.audioInstances[bufferInstance];
    if (!soundClip || soundClip.error) {
      return 2;
    }
    if (soundClip.buffer || soundClip.url) {
      return 0;
    }
    return 1;
  },
  _JS_Sound_Init() {
    try {
      window.AudioContext = window.AudioContext || window.webkitAudioContext;
      if (window.AudioContext) {
        WEBAudio.audioContext = new AudioContext();
      }
      if (wx && wx.createWebAudioContext) {
        WEBAudio.audioContext = wx.createWebAudioContext();
        console.log('use wx WebAudio');
      }
      if (!WEBAudio.audioContext) {
        err('Minigame Web Audio API not suppoted');
        return;
      }
      WEBAudio.audioWebSupport = 1;
      WEBAudio.audioWebEnabled = 1;
      wx.onHide(() => {
        WEBAudio.audioContext.suspend();
      });
      wx.onShow(() => {
        WEBAudio.audioContext.resume();
      });
      if (webAudioNeedResume) {
        let resumeInterval = 0;
        const tryToResumeAudioContext = function () {
          if (WEBAudio.audioContext.state === 'suspended' || WEBAudio.audioContext.state === 'interrupted') {
            WEBAudio.audioContext.resume();
            clearInterval(resumeInterval);
          }
        };
        setTimeout(() => {
          resumeInterval = setInterval(tryToResumeAudioContext, 2000);
        }, 2000);
      }
    } catch (e) {
      err('Web Audio API is not supported in this browser');
    }
  },
  _JS_Sound_IsStopped(channelInstance) {
    if (WEBAudio.audioWebEnabled == 0) {
      return true;
    }
    const channel = WEBAudio.audioInstances[channelInstance];
    if (!channel) {
      return true;
    }
    return channel.isStopped();
  },
  _JS_Sound_Load(ptr, length, decompress) {
    if (WEBAudio.audioWebEnabled === 0) {
      return 0;
    }
    const audioData = GameGlobal.unityNamespace.Module.HEAPU8.buffer.slice(ptr, ptr + length);

    // 超过128K强制使用innerAudio，低于128K使用webAudio
    if (length > 131072) {
      decompress = 0;
    } else {
      decompress = 1;
    }
    // 安卓和PC端强制用webAudio
    if (isAndroid || isPc) {
      decompress = 1;
    }

    if (decompress && WEBAudio.audioWebSupport) {
      const soundClip = {
        buffer: null,
        error: false,
        release() {
          this.buffer = null;
          audioBufferLength -= length;
        },
      };

      WEBAudio.audioContext.decodeAudioData(
        audioData,
        (buffer) => {
          soundClip.buffer = buffer;
          audioBufferLength += length;
        },
        (error) => {
          soundClip.error = true;
          console.log(`Decode error: ${error}`);
        },
      );
      WEBAudio.audioInstances[++WEBAudio.audioInstanceIdCounter] = soundClip;
    } else {
      const soundClip = {
        error: false,
        length: 441000,
        release() {
          audioBufferLength -= length;
          if (isSupportBufferURL) {
            wx.revokeBufferURL(this.url);
          }
          delete this.url;
        },
      };

      if (isSupportBufferURL) {
        const url = wx.createBufferURL(audioData);
        soundClip.url = url;
        audioBufferLength += length;
      } else {
        const tempFilePath = `${wx.env.USER_DATA_PATH}/__GAME_FILE_CACHE/audios/temp-audio${ptr + length}.mp3`;
        if (GameGlobal.manager.getCachePath(tempFilePath)) {
          soundClip.url = tempFilePath;
          audioBufferLength += length;
        } else {
          GameGlobal.manager.writeFile(tempFilePath, audioData).then(() => {
            soundClip.url = tempFilePath;
            audioBufferLength += length;
          })
            .catch((res) => {
              soundClip.error = true;
              err(res);
            });
        }
      }

      WEBAudio.audioInstances[++WEBAudio.audioInstanceIdCounter] = soundClip;
    }
    return WEBAudio.audioInstanceIdCounter;
  },
  _JS_Sound_Load_PCM(channels, length, sampleRate, ptr) {
    if (WEBAudio.audioWebSupport === 0 || WEBAudio.audioWebEnabled === 0) {
      return 0;
    }
    const sound = {
      buffer: WEBAudio.audioContext.createBuffer(channels, length, sampleRate),
      error: false,
    };
    for (let i = 0; i < channels; i++) {
      const offs = (ptr >> 2) + length * i;
      const { buffer } = sound;
      const copyToChannel = buffer.copyToChannel
        || function (source, channelNumber, startInChannel) {
          const clipped = source.subarray(0, Math.min(source.length, this.length - (startInChannel | 0)));
          this.getChannelData(channelNumber | 0).set(clipped, startInChannel | 0);
        };
      copyToChannel.apply(buffer, [GameGlobal.unityNamespace.Module.HEAPF32.subarray(offs, offs + length), i, 0]);
    }
    WEBAudio.audioInstances[++WEBAudio.audioInstanceIdCounter] = sound;
    return WEBAudio.audioInstanceIdCounter;
  },
  _JS_Sound_Play(bufferInstance, channelInstance, offset, delay) {
    if (WEBAudio.audioWebEnabled === 0) {
      return;
    }
    WXWASMSDK._JS_Sound_Stop(channelInstance, 0);
    const soundClip = WEBAudio.audioInstances[bufferInstance];
    const channel = WEBAudio.audioInstances[channelInstance];
    if (soundClip && soundClip.url) {
      try {
        channel.playUrl(
          delay,
          soundClip.url,
          offset,
          soundVolumeHandler[channelInstance],
          soundClip,
        );
      } catch (e) {
        err(`playUrl error. Exception: ${e}`);
      }
    } else if (soundClip && soundClip.buffer) {
      try {
        channel.playBuffer(
          WEBAudio.audioContext.currentTime + delay,
          soundClip.buffer,
          offset,
        );
      } catch (e) {
        err(`playBuffer error. Exception: ${e}`);
      }
    } else {
      console.log('Trying to play sound which is not loaded.');
    }
  },
  _JS_Sound_ReleaseInstance(instance) {
    if (WEBAudio.audioWebEnabled === 0) {
      return;
    }
    const object = WEBAudio.audioInstances[instance];
    if (object) {
      object.release();
    }
    delete WEBAudio.audioInstances[instance];
  },
  _JS_Sound_ResumeIfNeeded() {
    if (WEBAudio.audioWebSupport === 0 || WEBAudio.audioWebEnabled === 0) {
      return;
    }
    resumeWebAudio();
  },
  _JS_Sound_Set3D(channelInstance, threeD) {
    if (WEBAudio.audio3DSupport === 0 || WEBAudio.audioWebEnabled === 0) {
      return;
    }
    const channel = WEBAudio.audioInstances[channelInstance];
    if (channel.threeD != threeD) {
      channel.threeD = threeD;
      if (!channel.source) {
        channel.setup();
      }
      channel.setupPanning();
    }
  },
  _JS_Sound_SetListenerOrientation(x, y, z, xUp, yUp, zUp) {
    if (WEBAudio.audio3DSupport === 0 || WEBAudio.audioWebSupport === 0 || WEBAudio.audioWebEnabled === 0) {
      return;
    }
    x = x > 0 ? 0 : x;
    y = y > 0 ? 0 : y;
    z = z > 0 ? 0 : z;
    xUp = xUp < 0 ? 0 : xUp;
    yUp = yUp < 0 ? 0 : yUp;
    zUp = zUp < 0 ? 0 : zUp;
    if (
      x == WEBAudio.lOrientation.x
      && y == WEBAudio.lOrientation.y
      && z == WEBAudio.lOrientation.z
      && xUp == WEBAudio.lOrientation.xUp
      && yUp == WEBAudio.lOrientation.yUp
      && zUp == WEBAudio.lOrientation.zUp
    ) {
      return;
    }
    WEBAudio.lOrientation.x = x;
    WEBAudio.lOrientation.y = y;
    WEBAudio.lOrientation.z = z;
    WEBAudio.lOrientation.xUp = xUp;
    WEBAudio.lOrientation.yUp = yUp;
    WEBAudio.lOrientation.zUp = zUp;
    if (WEBAudio.audioContext.listener.forwardX) {
      WEBAudio.audioContext.listener.forwardX.setValueAtTime(-x, WEBAudio.audioContext.currentTime);
      WEBAudio.audioContext.listener.forwardY.setValueAtTime(-y, WEBAudio.audioContext.currentTime);
      WEBAudio.audioContext.listener.forwardZ.setValueAtTime(-z, WEBAudio.audioContext.currentTime);
      WEBAudio.audioContext.listener.upX.setValueAtTime(xUp, WEBAudio.audioContext.currentTime);
      WEBAudio.audioContext.listener.upY.setValueAtTime(yUp, WEBAudio.audioContext.currentTime);
      WEBAudio.audioContext.listener.upZ.setValueAtTime(zUp, WEBAudio.audioContext.currentTime);
    } else {
      WEBAudio.audioContext.listener.setOrientation(-x, -y, -z, xUp, yUp, zUp);
    }
  },
  _JS_Sound_SetListenerPosition(x, y, z) {
    if (WEBAudio.audio3DSupport === 0 || WEBAudio.audioWebSupport === 0 || WEBAudio.audioWebEnabled === 0) {
      return;
    }
    x = x < 0 ? 0 : x;
    y = y < 0 ? 0 : y;
    z = z < 0 ? 0 : z;
    if (x == WEBAudio.lPosition.x && y == WEBAudio.lPosition.y && z == WEBAudio.lPosition.z) {
      return;
    }
    WEBAudio.lPosition.x = x;
    WEBAudio.lPosition.y = y;
    WEBAudio.lPosition.z = z;
    if (WEBAudio.audioContext.listener.positionX) {
      WEBAudio.audioContext.listener.positionX.setValueAtTime(x, WEBAudio.audioContext.currentTime);
      WEBAudio.audioContext.listener.positionY.setValueAtTime(y, WEBAudio.audioContext.currentTime);
      WEBAudio.audioContext.listener.positionZ.setValueAtTime(z, WEBAudio.audioContext.currentTime);
    } else {
      WEBAudio.audioContext.listener.setPosition(x, y, z);
    }
  },
  _JS_Sound_SetLoop(channelInstance, loop) {
    if (WEBAudio.audioWebEnabled === 0) {
      return;
    }
    const channel = WEBAudio.audioInstances[channelInstance];
    if (!channel.source) {
      channel.setup();
    }
    channel.source.loop = loop > 0;
  },
  _JS_Sound_SetLoopPoints(channelInstance, loopStart, loopEnd) {
    if (WEBAudio.audioWebEnabled === 0) {
      return;
    }
    const channel = WEBAudio.audioInstances[channelInstance];
    if (!channel.source) {
      channel.setup();
    }
    channel.source.loopStart = loopStart;
    channel.source.loopEnd = loopEnd;
  },
  _JS_Sound_SetPaused(channelInstance, paused) {
    if (WEBAudio.audioWebEnabled === 0) {
      return;
    }
    const channel = WEBAudio.audioInstances[channelInstance];
    if (!!paused !== channel.isPaused()) {
      if (paused) {
        channel.pause();
      } else {
        channel.resume();
      }
    }
  },
  _JS_Sound_SetPitch(channelInstance, v) {
    if (WEBAudio.audio3DSupport === 0 || WEBAudio.audioWebSupport === 0 || WEBAudio.audioWebEnabled === 0) {
      return;
    }
    try {
      WEBAudio.audioInstances[channelInstance].source.setPitch(v);
    } catch (e) {
      err(`Invalid audio pitch ${v} specified to WebAudio backend!`);
    }
  },
  _JS_Sound_SetPosition(channelInstance, x, y, z) {
    if (WEBAudio.audio3DSupport === 0 || WEBAudio.audioWebSupport === 0 || WEBAudio.audioWebEnabled === 0) {
      return;
    }
    console.error('不支持3d音效');
  },
  _JS_Sound_SetVolume(channelInstance, v) {
    if (WEBAudio.audioWebEnabled === 0) {
      return;
    }
    try {
      const volume = Number(v.toFixed(2));
      const cur = soundVolumeHandler[channelInstance];
      if (cur === volume) {
        return;
      }
      // 和默认值一样
      if (cur == undefined && v == 1) {
        return;
      }
      soundVolumeHandler[channelInstance] = volume;
      const channel = WEBAudio.audioInstances[channelInstance];
      channel.setVolume(volume);
    } catch (e) {
      err(`Invalid audio volume ${v} specified to WebAudio backend!`);
    }
  },
  _JS_Sound_Stop(channelInstance, delay) {
    if (WEBAudio.audioWebEnabled === 0) {
      return;
    }
    const channel = WEBAudio.audioInstances[channelInstance];
    channel.stop(delay);
  },
  resumeWebAudio,
};

// 声音被打断后自动帮用户恢复
const HandleInterruption = {
  init() {
    let INTERRUPT_LIST = {};
    wx.onHide(() => {
      for (const key in audios) {
        if (!audios[key].paused !== false) {
          INTERRUPT_LIST[key] = true;
        }
      }
    });
    wx.onShow(() => {
      for (const key in audios) {
        if (audios[key].paused !== false && INTERRUPT_LIST[key]) {
          audios[key].play();
        }
      }
      INTERRUPT_LIST = {};
    });
    wx.onAudioInterruptionBegin(() => {
      for (const key in audios) {
        if (!audios[key].paused !== false) {
          INTERRUPT_LIST[key] = true;
        }
      }
    });
    wx.onAudioInterruptionEnd(() => {
      for (const key in audios) {
        if (audios[key].paused !== false && INTERRUPT_LIST[key]) {
          audios[key].play();
        }
      }
      INTERRUPT_LIST = {};

      resumeWebAudio();
    });
  },
};

HandleInterruption.init();
