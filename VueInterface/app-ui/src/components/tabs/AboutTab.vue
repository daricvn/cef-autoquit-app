<template>
    <div class="column">
        <div class="col-auto text-center">
            <h5 v-text="ui?ui.about:'About'" class="q-mt-sm q-mb-none"></h5>
            <div class="text-caption">version: {{ appSettings.version }}</div>
            <q-separator class="q-ma-sm" />
        </div>
        <div class="col-auto">
            <span class="text-body2">
                Autoquit is an open-source application which maintained by Darick Nguyen. Written in C# and Vue.<br />
                You can use for free as it always be. No license key or subscription fee is required to use this application <br />
                This application is also free-ads. If you see advertisement in the application, make sure you downloaded its build from the official website:
                <a href="https://daricvn.github.io/autoquit2">https://daricvn.github.io/autoquit2</a> <br />
            </span>
        </div>
        <div class="col-auto text-center">
            <a href="https://fb.com/daricvn" target="_blank" rel="noopener noreferrer">
                <q-icon name="fab fa-facebook-square" color="blue" size="32px" class="q-mt-sm q-mr-sm"></q-icon>
            </a>
            <a href="https://github.com/daricvn" target="_blank" rel="noopener noreferrer">
                <q-icon name="fab fa-github" color="grey-9" size="32px" class="q-mt-sm q-mr-sm"></q-icon>
            </a>
        </div>
        <div class="col-auto q-mt-md">
            <div class="text-subtitle2 q-mb-sm" v-text="ui? ui.help_title:'Help section'"></div>
            <div class="column">
                <div class="col">
                    <q-btn flat rounded class="text-blue">
                        {{ ui ? ui.help_inputtype : "Getting Started"}}
                        <q-icon class="on-right" name="help_outline"></q-icon>
                    </q-btn>
                </div>
                <div class="col">
                    <q-btn flat rounded class="text-blue">{{ ui ? ui.help_manipulatemode : "Manipulate Mode"}}</q-btn>
                </div>
                <div class="col">
                    <q-btn flat rounded class="text-blue">{{ ui ? ui.help_wrongcoordinate : "Wrong Coordinate"}}</q-btn>
                </div>
                <div class="col">
                    <q-btn flat rounded class="text-blue">{{ ui ? ui.help_security : "Security"}}</q-btn>
                </div>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
import { Vue, Component }  from 'vue-property-decorator'
import { State } from 'vuex-class'
import { AppSettings } from '../../models/AppSettings';

@Component
export default class AboutTab extends Vue{
    @State("lang") ui: any;
    @State("settings") appSettings: AppSettings | undefined;
}
</script>