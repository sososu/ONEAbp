
import axios,{ AxiosRequestConfig } from 'axios'



const pending = {}; // 请求队列

// 增加到等待队列
export const addPengding = (config) => {
    const cancelKey = `${config.method}-${config.url}`;
    config.cancelToken = new axios.CancelToken((c) => {
        pending[cancelKey] = c;
    })
}

// 移出等待队列
export const removePending = (config) => {
    const cancelKey = `${config.method}-${config.url}`;
    if (pending[cancelKey]) {
        let cancel = pending[cancelKey];
        cancel();
        delete pending[cancelKey];
    }
}

// 所有等待移出队列
export const cancelAllPening = () => {
    Object.values(pending).forEach((cancel) => {
        cancel();
    })
}
