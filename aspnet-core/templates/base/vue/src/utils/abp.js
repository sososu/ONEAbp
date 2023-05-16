import store from '@/store'
import router from '@/router'

const baseListQuery = {
  page: 1,
  limit: 20,
  sort: undefined,
  filter: undefined
}

export const httpCode = [
  {
    label: 200,
    value: 200
  },
  {
    label: 401,
    value: 401
  },
  {
    label: 403,
    value: 403
  },
  {
    label: 500,
    value: 500
  }
]

export function transformAbpListQuery(query) {
  query.filter = query.filter === '' ? undefined : query.filter

  const abpListQuery = {
    maxResultCount: query.limit,
    skipCount: (query.page - 1) * query.limit,
    sorting:
      query.sort && query.sort.endsWith('ending')
        ? query.sort.replace('ending', '')
        : query.sort,
    ...query
  }

  delete abpListQuery.page
  delete abpListQuery.limit
  delete abpListQuery.sort

  return abpListQuery
}


export default baseListQuery
