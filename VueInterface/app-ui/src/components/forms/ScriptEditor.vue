<script lang="ts">
import Vue from 'vue';
import { ScriptType } from '../../models/ScriptItem';

export default class ScriptEditor extends Vue{

    getScriptType(type: ScriptType){
        if (type == ScriptType.ENTER_TEXT || type == ScriptType.RANDOM_TEXT)
            return 'textarea';
        if (type == ScriptType.ENTER_SECRET)
            return 'password';
        if (type == ScriptType.KEY_UP || type == ScriptType.KEY_DOWN || type == ScriptType.KEY_PRESS)
            return 'key';
        if (type == ScriptType.MOUSE_UP || type == ScriptType.MOUSE_DOWN || type == ScriptType.MOUSE_CLICK)
            return 'mouse';
        if (type == ScriptType.FROM_FILE)
            return 'file';
        return null;
    }
}
</script>