<template>
  <div class="logout">正在登出...</div>
</template>

<script setup>
import { useRoute, useRouter } from "vue-router";
import useUserStore from '@/store/modules/user'

const userStore = useUserStore()
const router = useRouter();

let logoutId = route.query.logoutId;

useUserStore.logout(logoutId).then(res => {
  if (res.postLogoutRedirectUri == null) {
    router.push({ name: "index" });
  } else {
    window.location.href = res.postLogoutRedirectUri;
  }})

</script>
