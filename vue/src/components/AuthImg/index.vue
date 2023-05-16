<template>
  <img ref="imgRef" />
</template>


<script setup>
import { computed, onMounted, ref } from 'vue';
import useUserStore from "@/store/modules/user";
import { defineEmits,watch} from 'vue'
const userStore = useUserStore();
const { proxy } = getCurrentInstance();
const imgRef=ref();
const props = defineProps({
  authSrc: {
  type: String,
  default: "",
  required: false,
},
});


const emit = defineEmits(["setImageData"])

watch(() => props.authSrc, (newValue, oldValue) => {
 setSrc(newValue)
})

onMounted(() => {
  
    Object.defineProperty(Image.prototype, 'authSrc', {
      writable: true,
      enumerable: true,
      configurable: true
    })
    setSrc(props.authSrc)
  }
);

function setSrc(vsrc){
let img = imgRef.value;
    let request = new XMLHttpRequest();
    request.responseType = 'blob';
    request.open('get', vsrc, true);
    request.setRequestHeader("Authorization", 'Bearer '+userStore.token);
    request.onreadystatechange = e => {
      if (request.readyState == XMLHttpRequest.DONE && request.status == 200) {
        img.src = URL.createObjectURL(request.response);
        emit("setImageData",request.response)
        img.onload = () => {
          URL.revokeObjectURL(img.src);
        }
      }
    };
    request.send(null);
}

</script>