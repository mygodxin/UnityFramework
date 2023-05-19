import moduleHelper from './module-helper';
import { formatJsonStr } from './sdk';

const tempCacheObj = {};

export default {
  WXCameraCreateCamera(conf, callbackId) {
    const obj = wx.createCamera({
      ...formatJsonStr(conf),
      success(res) {
        moduleHelper.send('CameraCreateCallback', JSON.stringify({
          callbackId, type: 'success', res: JSON.stringify(res),
        }));
      },
      fail(res) {
        moduleHelper.send('CameraCreateCallback', JSON.stringify({
          callbackId, type: 'fail', res: JSON.stringify(res),
        }));
      },
      complete(res) {
        moduleHelper.send('CameraCreateCallback', JSON.stringify({
          callbackId, type: 'complete', res: JSON.stringify(res),
        }));
      },
    });
    this.CameraList = this.CameraList || {};
    const list = this.CameraList;
    list[callbackId] = obj;
  },
  WXCameraCloseFrameChange(id) {
    const obj = this.CameraList[id];
    if (obj) {
      obj.closeFrameChange();
    }
  },
  WXCameraDestroy(id) {
    const obj = this.CameraList[id];
    if (obj) {
      obj.destroy();
    }
  },
  WXCameraListenFrameChange(id) {
    const obj = this.CameraList[id];
    if (obj) {
      obj.listenFrameChange();
    }
  },
  WXCameraOnAuthCancel(id) {
    const obj = this.CameraList[id];
    obj.OnAuthCancelList = obj.OnAuthCancelList || [];
    const callback = (res) => {
      const resStr = JSON.stringify({
        callbackId: id,
        res: JSON.stringify(res),
      });
      moduleHelper.send('CameraOnAuthCancelCallback', resStr);
    };
    obj.OnAuthCancelList.push(callback);
    obj.onAuthCancel(callback);
  },
  WXCameraOnCameraFrame(id) {
    const obj = this.CameraList[id];
    obj.OnCameraFrameList = obj.OnCameraFrameList || [];
    const callback = (result) => {
      tempCacheObj[id] = result.data;
      const resStr = JSON.stringify({
        callbackId: id,
        res: JSON.stringify({
          width: result.width,
          height: result.height,
        }),
      });
      moduleHelper.send('CameraOnCameraFrameCallback', resStr);
    };
    obj.OnCameraFrameList.push(callback);
    obj.onCameraFrame(callback);
  },
  WXCameraOnStop(id) {
    const obj = this.CameraList[id];
    obj.OnStopList = obj.OnStopList || [];
    const callback = (res) => {
      const resStr = JSON.stringify({
        callbackId: id,
        res: JSON.stringify(res),
      });
      moduleHelper.send('CameraOnStopCallback', resStr);
    };
    obj.OnStopList.push(callback);
    obj.onStop(callback);
  },
  WXCameraArrayBuffer(buffer, offset, callbackId) {
    buffer.set(new Uint8Array(tempCacheObj[callbackId]), offset);
    delete tempCacheObj[callbackId];
  },
};
