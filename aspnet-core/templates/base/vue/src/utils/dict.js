import useDictStore from '@/store/modules/dict'
import {
  getDataDictionaryItemsByCode
} from "@/api/system/dictionaries";

/**
 * 获取字典数据
 */
export function useDict(...args) {
  const res = ref({});
  return (() => {
    args.forEach((code, index) => {
      res.value[code] = [];
      const dicts = useDictStore().getDict(code);
      if (dicts) {
        res.value[code] = dicts;
      } else {
        getDataDictionaryItemsByCode({code:code}).then(resp => {
          res.value[code] = resp.items.map(p => ({ label: p.name, value: p.value, elTagType: p.code, elTagClass: p.code }))
          useDictStore().setDict(code, res.value[code]);
        })
      }
    })
    return toRefs(res.value);
  })()
}