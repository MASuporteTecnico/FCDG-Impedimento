<style scoped>
:deep(.blinking-class) {
  animation: blink-animation 1s infinite;
}

@keyframes blink-animation {
  0%,
  49%,
  100% {
    opacity: 1;
  }
  50%,
  99% {
    opacity: 0;
  }
}
</style>

<template>
  <span title="Tempo de sessão">
    <v-btn stacked :class="isBlinking ? 'blinking-class' : ' '">
      <v-badge :color="isBlinking ? 'error' : 'info'" :content="Time.value">
        <v-icon>mdi-clock-outline</v-icon>
      </v-badge>
    </v-btn>
  </span>
  <Confirm Msg="Você será desconectado." ConfirmTxt="Continuar conectado." NoCancel :Show="ShowDialog" @confirm="ShowDialog = !ShowDialog"></Confirm>
</template>

<script setup>
defineProps({
  Seconds: { type: Number, default: 1200 },
  Enabled: { type: Boolean, default: false },
  Page: { type: String, default: "/logout" },
  ShowDialog: { type: Boolean, default: false },
});

import moment from "moment";
import { useRouter } from "vue-router";
import { useAppStore } from "@/stores/app";
import { onUnmounted, onMounted, ref, watch, reactive } from "vue";
import { useInterval } from "../composables/useInterval";

const router = useRouter();
const store = useAppStore();

const isBlinking = computed(() => {
  return Secs.value >= 180 ? false : true;
});

let Interval = ref();
let Secs = ref(null);
let Activated = ref(false);
let Time = ref(0);
let LastTime = 0;
let TimeCompare = 0;

const CountDownLogoffReset = computed(() => {
  return store.CountDownLogoffReset;
});

watch(CountDownLogoffReset, () => {
  Secs.value = Seconds;
});

watch(() => Enabled, (newValue, oldValue)  => {
  Secs.value = Seconds;
});


function CountDown() {
  
  if (Activated.value == true) {
    Secs.value--;
    console.log(Secs.value);

    let t = moment.duration(Secs.value, "seconds");
    Time.value = `${t.get("minutes").toString()}:${t.get("seconds").toString().padStart(2, 0)}`;

    var tempTimeStamp = new Date();
    var TimeStampNow = tempTimeStamp.getTime();

    if (LastTime == null) {
      LastTime = TimeStampNow;
    }

    if (Secs <= 0) {
      Activated.value = false;
      router.push(Page);
    }
  }
}

onMounted(() => {
  // Secs.value = Seconds;
  // Activated.value = Enabled;
  //setTimer();
  useInterval(CountDown, 1000).start();
});

</script>