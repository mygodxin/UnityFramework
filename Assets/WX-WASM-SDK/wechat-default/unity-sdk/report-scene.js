/* eslint-disable no-param-reassign */
import { formatJsonStr, formatResponse } from './sdk';
import moduleHelper from './module-helper';

export default {
  WXReportScene(conf, callbackId) {
    conf = formatJsonStr(conf);
    if (wx.reportScene) {
      if (GameGlobal.manager && GameGlobal.manager.setGameStage) {
        GameGlobal.manager.setGameStage(conf.sceneId);
      }
      wx.reportScene({
        ...conf,
        success(res) {
          formatResponse('GeneralCallbackResult', res);
          moduleHelper.send('ReportSceneCallback', JSON.stringify({
            callbackId, type: 'success', res: JSON.stringify(res),
          }));
        },
        fail(res) {
          formatResponse('GeneralCallbackResult', res);
          moduleHelper.send('ReportSceneCallback', JSON.stringify({
            callbackId, type: 'fail', res: JSON.stringify(res),
          }));
        },
        complete(res) {
          formatResponse('GeneralCallbackResult', res);
          moduleHelper.send('ReportSceneCallback', JSON.stringify({
            callbackId, type: 'complete', res: JSON.stringify(res),
          }));
        },
      });
    }
  },
};
