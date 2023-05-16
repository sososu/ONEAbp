<template>
<div>正在登录...</div>
</template>

<script setup>
import {
    useOidcLogout
} from "@/libs/oidcClientConfig";
import useUserStore from '@/store/modules/user'

const userStore = useUserStore()
const router = useRouter();

userStore.login().then(() => {
   console.log(router)
   router.push({ path: "/" });

}).catch(error => {
    console.log("callback err:" + error)
    useOidcLogout()
});
</script>
