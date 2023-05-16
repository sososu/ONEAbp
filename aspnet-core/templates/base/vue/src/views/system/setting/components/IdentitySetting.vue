<template>
    <div class="app-container">
    <h2 class="title">密码设置</h2>
    <el-form ref="appRef" :model="form" :rules="rules"  label-position="top">
            <el-form-item label="要求长度" prop="requiredLength">
                <el-input v-model.trim="form.password.requiredLength" type="number" :min="6" :max="18" placeholder="请输入" />
                <span class="tips">密码的最小长度</span>
            </el-form-item>
            <el-form-item label="要求唯一字符数量" prop="requiredUniqueChars">
                <el-input v-model.trim="form.password.requiredUniqueChars" type="number" :min="0" :max="18" placeholder="请输入" />
                <span class="tips">密码必须包含唯一字符的数量</span>
            </el-form-item>
            <el-form-item  prop="requireNonAlphanumeric">
                <el-checkbox v-model="form.password.requireNonAlphanumeric" label="要求非字母数字"/>
                <span class="tips">密码是否必须包含非字母数字</span>
            </el-form-item>
            <el-form-item prop="requireLowercase">
                <el-checkbox v-model="form.password.requireLowercase" label="要求小写字母" />
                <span class="tips">密码是否必须包含小写字母</span>
            </el-form-item>
             <el-form-item  prop="requireUppercase">
                <el-checkbox v-model="form.password.requireUppercase"  label="要求大写字母"/>
                <span class="tips">密码是否必须包含大写字母</span>
            </el-form-item>

            <el-form-item  prop="requireDigit" >
                <el-checkbox v-model="form.password.requireDigit" label="要求数字"/>
                <span class="tips">密码是否必须包含数字</span>
            </el-form-item>

            <el-form-item label="默认密码" prop="password.defaultPassword">
                <el-input v-model.trim="form.password.defaultPassword" placeholder="请输入" />
                <span class="tips">密码必须符号上面要求</span>
            </el-form-item>
    </el-form>
        <div class="dialog-footer">
                <el-button type="primary" @click="submitForm" v-hasPermi="['SettingManagement.IdentitySettings.Update']">保存</el-button>
        </div>
    </div>
</template>

<script setup>
import {
    getIdentitySetting,
    setIdentitySetting
} from "@/api/system/setting";
import{deepClone}from "@/utils/toolFn"


const { proxy } = getCurrentInstance();
const open = ref(false);
const loading = ref(true);


const data = reactive({
    form: {password:{defaultPassword:""}},
    rules: {
        'password.defaultPassword': [{
            required: true,
            message: "默认密码不能为空",
            trigger: "blur"
        }]
    },
});
const {
    form,
    rules
} = toRefs(data);


/** 查询应用列表 */
function getSetting() {
    loading.value = true;
    getIdentitySetting().then(response => {
        form.value = response;
        loading.value = false;
    });
}

/** 提交按钮 */
function submitForm() {
    proxy.$refs["appRef"].validate(valid => {
        if (valid) {
            setIdentitySetting(form.value).then(response => {
                    proxy.$modal.msgSuccess("提交成功");
                });
        }
    });
}
getSetting();

</script>

<style lang='scss' scoped>

.app-container{
      .title{
        margin-top: -10px;
      }
        .tips{
                    color: #9dabb6;
                    font-size: 12px;
                    margin-left: 10px;
                }
}

</style>