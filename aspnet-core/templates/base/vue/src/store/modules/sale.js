import {
    createSaleVersion,
    updateSaleVersion,
    deleteSaleVersion,
    getSaleVersions
} from "@/api/sass/sale";
  
const useSaleStore = defineStore("sale", {
    state: () => ({
      saleVersions: [],
    }),
    actions: {
      modifySale(id,data) {
          return new Promise((resolve, reject) => {
            updateSaleVersion(id,data)
          .then((res) => {
            this.listSale();
            resolve(res);
          })
          .catch((error) => {
            reject(error);
          });
      })},
  
      addSale(data) {
          return new Promise((resolve, reject) => {
            createSaleVersion(data)
          .then((res) => {
            this.listSale();
            resolve(res);
          })
          .catch((error) => {
            reject(error);
          });
      })},
  
      removeSale(id) {
          return new Promise((resolve, reject) => {
            deleteSaleVersion(id)
          .then((res) => {
            this.listSale();
            resolve(res);
          })
          .catch((error) => {
            reject(error);
          });
      })},
  
      listSale() {
          return new Promise((resolve, reject) => {
            getSaleVersions({
                pageIndex: 1,
                pageSize: 1000
            }).then(res => {
                this.saleVersions = res.items;
                resolve(res)
            }).catch((error) => {
            reject(error);
          });
      })}
    },
  });
  
  export default useSaleStore;
  
  