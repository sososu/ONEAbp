<template>
    <div class="error_container">
        <div v-if="errorId">
            <h1 class="mb20">Error</h1>
            <div v-for="key in Object.keys(errorData)"
                 :key="key"
                 class="content_item">
                <div v-if="errorData[key]">
                    {{key}}: <span>{{errorData[key]}}</span>
                </div>
            </div>
        </div>
        <div v-else>该页面不存在</div>
    </div>
</template>

<script setup>
import { ref } from 'vue'
import {
loginError
} from "@/api/login";
import { getUrlParam } from '@/utils/toolFn';


const actionsList = [
    'system/loginError',
]
let { errorId } = getUrlParam();
let errorData = ref({
    errorDescription: null,
});


if (errorId) loginError({ errorId }).then((res) => res && (errorData.value = res.data))


</script>
<style lang="scss" scoped>
.error_container {
    padding: 30px;

    .content_item {
        span {
            margin-left: 10px;
            font-size: 14px;
            color: #666;
        }
    }
}
</style>