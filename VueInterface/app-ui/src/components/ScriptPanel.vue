<template>
    <div>
        <div class="row">
            <q-select @popup-show="onDropdownOpen" style="width: 100%;" square outlined v-model="model" :options="options" :label="!!ui?ui.process:'Process'" :disable="!options || options.lenght==0" />
        </div>
    </div>
</template>
<script lang="ts">
import { Vue, Component, InjectReactive } from 'vue-property-decorator'

@Component
export default class ScriptPanel extends Vue{
    options: Array<String>=["Option 1","Option 2","Option 3"];
    model: String ="Option 1";
    @InjectReactive("lang") ui: any;
    mounted(){

    }

    onDropdownOpen(){
    }
}
</script>