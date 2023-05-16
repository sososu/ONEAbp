import {
  queryOrganizations,
  deleteOrganization,
  createOrganization,
  updateOrganization,
  getOrganizationExclude,
  getOrganizationtTree,
} from "@/api/system/organization";
import { deepClone } from "@/utils/toolFn";

const useDeptStore = defineStore("dept", {
  state: () => ({
    deptTree: [],
  }),
  actions: {
    modifyOrganization(data) {
        return new Promise((resolve, reject) => {
      updateOrganization(data)
        .then((res) => {
          this.listOrganizationtTree();
          resolve(res);
        })
        .catch((error) => {
          reject(error);
        });
    })},

    addOrganization(data) {
        return new Promise((resolve, reject) => {
      createOrganization(data)
        .then((res) => {
          this.listOrganizationtTree();
          resolve(res);
        })
        .catch((error) => {
          reject(error);
        });
    })},

    removeOrganization(id) {
        return new Promise((resolve, reject) => {
      deleteOrganization(id)
        .then((res) => {
          this.listOrganizationtTree();
          resolve(res);
        })
        .catch((error) => {
          reject(error);
        });
    })},

    listOrganizationtTree() {
        return new Promise((resolve, reject) => {
      getOrganizationtTree()
        .then((res) => {
          this.deptTree = res.items;
          resolve(res);
        })
        .catch((error) => {
          reject(error);
        });
    })}
  },
});

export default useDeptStore;
