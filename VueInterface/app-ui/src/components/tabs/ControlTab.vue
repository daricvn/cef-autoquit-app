<template>
    <div class="row">
        <div class="col-auto q-pr-sm" style="line-height: 40px;">
            Playback Speed:
        </div>
        <div class="col q-pr-md">
            <q-slider markers :min="0.5" :max="2" :step="0.25" v-model.lazy="playerState.speed"
                :readonly="playerState.play || playerState.record"
                label
                :label-value="playerState.speed*100 + '%'"
                ></q-slider>
        </div>
    </div>
</template>

<script>
import { Vue, Component } from 'vue-property-decorator'
import { State } from 'vuex-class'

@Component
export default class ControlTab extends Vue{
    @State("player") playerState;
}
</script>