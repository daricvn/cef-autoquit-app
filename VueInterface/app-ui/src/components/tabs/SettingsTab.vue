<template>
    <div class="column">
        <div class="col-auto text-center">
            <h6 class="q-mt-md q-mb-lg">{{ ui ? ui.settings :'Settings'}}</h6>
        </div>
        <div class="col-auto">
            <div class="row">
                <div class="col-auto"><span v-text="ui?ui.darkmode:'Dark Theme'"></span></div>
                <div class="col q-pl-md">
                    <q-toggle dense :value="darkTheme" @input="onDarkThemeChanged"></q-toggle>
                </div>
            </div>
        </div>
    </div>
</template>
<script lang="ts">
import { Vue, Component, Watch } from 'vue-property-decorator'
import { State, Mutation } from 'vuex-class'
import { PlayerState } from '../../models/PlayerState';

@Component
export default class SettingsTab extends Vue{
    @State("lang") ui :any;
    @State("darkTheme") darkTheme: any;
    @Mutation("setDarkTheme") setDarkTheme: any;
    mounted(){

    }

    onDarkThemeChanged(){
        this.setDarkTheme(!this.darkTheme);
    }
}
</script>