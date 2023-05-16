
/**
 * @description: 获取地址栏参数
 * @param {} AnyObject
 * @return {object}
 */
export const getUrlParam = (url = window.location.href) => {
    const urlParamArr = /.+\?(.+?)([\/#].+|$)/.exec(url);
    let paramsData = {};
    if (urlParamArr) {
        let urlParam = urlParamArr[1];
        paramsData = urlParam
            .split('&')
            .reduce((result, item) => {
                let keyValue = item.split('=');
                result[keyValue[0]] = keyValue[1];
                return result;
            }, {});
    }
    return paramsData;
};

/**
 * @description: 拼接地址参数
 * @param {string} url 地址
 * @param {AnyObject} params 对象参数
 * @return {string}
 */
export const setUrlParams = (url, params) => {
    const str = Object.keys(params)
        .map((key) => {
            return `${key}=${params[key]}`;
        })
        .join('&');
    return url + (url.indexOf('?') > -1 ? '&' : '?') + str;
};

/**
 * @description: 时间格式化
 * @param {Date} time 支持new Date的时间
 * @param {string} fmt
 * @return {string}
 */
export const formatTime = (
    time,
    fmt
)=> {
    if (time == null) return '';
    let _fmt = fmt ? fmt : 'yyyy-MM-dd hh:mm:ss';
    let _time = new Date(time);
    let z= {
        M: _time.getMonth() + 1,
        d: _time.getDate(),
        h: _time.getHours(),
        m: _time.getMinutes(),
        s: _time.getSeconds(),
    };
    _fmt = _fmt.replace(/(M+|d+|h+|m+|s+)/g, function (v) {
        return ((v.length > 1 ? '0' : '') + z[v.slice(-1)]).slice(-2);
    });
    let result = _fmt.replace(/(y+)/g, function (v) {
        return _time.getFullYear().toString().slice(-v.length);
    });
    return result;
};

/**
 * @description:深拷贝
 * @param { AnyObject} obj
 * @return {AnyObject}
 */
export const deepClone = (obj) => {
    let objClone = Array.isArray(obj) ? [] : {};
    if (obj && typeof obj === 'object') {
        for (let key in obj) {
            if (Object.prototype.hasOwnProperty.call(obj, key)) {
                //判断ojb子元素是否为对象，如果是，递归复制
                if (obj[key] && typeof obj[key] === 'object') {
                    objClone[key] = deepClone(obj[key]);
                } else {
                    //如果不是，简单复制
                    objClone[key] = obj[key];
                }
            }
        }
    }
    return objClone;
};
