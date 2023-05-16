<template>
<div>
    <el-button @click="addGroup" icon="Plus" round></el-button>
    <el-form ref="form" :model="form" label-width="80px">
        <el-form-item v-for="(group, index) in form.conditionGroups" :key="index">
            
            <el-card class="box-card">

                <template #header>
      <div class="card-header">
            <el-input v-model="group.name" placeholder="请输入条件组名称"></el-input>
            <el-button icon="Close"   circle  @click="deleteGroup(index)"  style="margin-left: 20px;"></el-button>
      </div>
    </template>

            <!-- <el-row>
                <el-col :span="20">
                    <el-input v-model="group.name" placeholder="请输入条件组名称"></el-input>
                </el-col>
                <el-col :span="2">
                    <el-button icon="Delete" type="danger" text @click="deleteGroup(index)"></el-button>
                </el-col>
            </el-row> -->
            <el-row>
                <el-button icon="Plus" size="small"  @click="addCondition(index)"></el-button>
            </el-row>
            <el-row>
                <el-col :span="24">
                    <el-form-item v-for="(onditionUnit, i) in group.conditionUnits" :key="i">
                        <el-row>
                            <el-col :span="7">
                                <el-select v-model="onditionUnit.condition.fieldName" placeholder="请选择字段">
                                    <el-option v-for="fieldName in fieldNames" :key="fieldName.name" :label="fieldName.displayName"  :value="fieldName.name" :title="fieldName.description">                                    
                                    </el-option>
                                </el-select>
                            </el-col>
                            <el-col :span="7">
                                <el-select v-model="onditionUnit.condition.compare" placeholder="请选择比较方式">
                                    <el-option v-for="compare in QueryCompare" :key="compare.value" :label="compare.label" :value="compare.value"></el-option>
                                </el-select>
                            </el-col>
                            <el-col :span="8">

                  
                                <el-input v-if="defineFields" 
      v-model="onditionUnit.condition.fieldValue"
      placeholder="请输入值 数组用','分隔"
      class="input-with-select"
    >
      <template #append>
        <el-select  placeholder="预值" @change="changeDefineFields(index,i,$event)" style="width: 90px">
            <el-option v-for="fields in defineFields" :key="fields" :label="fields" :value="fields"></el-option>
        </el-select>
      </template>
    </el-input>

    <el-input v-else  v-model="onditionUnit.condition.fieldValue" placeholder="请输入值"></el-input>

                            </el-col>
                            <el-col :span="2">
                                <el-button icon="Minus" size="small" @click="deleteCondition(index, i)"></el-button>
                            </el-col>
                        </el-row>
                        <el-row>
                            <el-col :span="15">
                                <el-select v-model="onditionUnit.conditionOperator" placeholder="请选择运算符">
                                    <el-option v-for="relation in ConditionOperator" :key="relation.value" :label="relation.label" :value="relation.value"></el-option>
                                </el-select>
                            </el-col>
                        </el-row>
                    </el-form-item>
                </el-col>
            </el-row>
            </el-card>

            <el-select v-model="group.conditionOperator" placeholder="请选择运算符" style="margin-top:10px">
                <el-option v-for="relation in ConditionOperator" :key="relation.value" :label="relation.label" :value="relation.value"></el-option>
            </el-select>

        </el-form-item>
    </el-form>
</div>
</template>

<script setup>
const {
    proxy
} = getCurrentInstance();
const {
    QueryCompare
} = proxy.useDict("QueryCompare");

// const {
//     RuleDataOperation
// } = proxy.useDict("RuleDataOperation");

const {
    ConditionOperator
} = proxy.useDict("ConditionOperator");

const props = defineProps({
    fieldNames: {
        type: Array,
        require: true
    },
    form:{
        type: Object,
        require:true
    },
    defineFields:{
        typeof: Array
    }
})

const selectedDefineFields='';

const emits = defineEmits(["update:fieldNames", "update:form","update:defineFields"]); 
// const data = reactive({
//     form: {
//         conditionGroups: [{
//             name: '',
//             conditionUnits: [{
//                 condition:{
//                 fieldName: '',
//                 compare: '',
//                 fieldValue: ''
//                 },
//                 conditionOperator: null
//             }],
//             conditionOperator: null
//         }]
//     }
// })
// const {
//     form
// } = toRefs(data);

function changeDefineFields(groupIndex,conditionIndex,val){
    props.form.conditionGroups[groupIndex].conditionUnits[conditionIndex].condition.fieldValue=val;
}

function addGroup() {
    props.form.conditionGroups??=[]
    props.form.conditionGroups.push({
            name: '',
            conditionUnits: [{
                condition:{
                fieldName: '',
                compare: '',
                fieldValue: ''
                },
                conditionOperator: null
            }],
            conditionOperator: null
        });
}

function deleteGroup(index) {
    props.form.conditionGroups.splice(index, 1);
}

function addCondition(groupIndex) {
    props.form.conditionGroups[groupIndex].conditionUnits.push({
                condition:{
                fieldName: '',
                compare: '',
                fieldValue: ''
                },
                conditionOperator: null
            });
}

function deleteCondition(groupIndex, conditionIndex) {
    props.form.conditionGroups[groupIndex].conditionUnits.splice(conditionIndex, 1);
}
</script>
<style lang="scss">
.box-card{
    margin-top: 15px;
}
.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.el-col {
  border-radius: 4px;
  padding: 5px;
}


</style>