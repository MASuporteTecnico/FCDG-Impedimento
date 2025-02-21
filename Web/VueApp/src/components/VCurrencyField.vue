<script setup lang="ts">
import { useCurrencyInput } from 'vue-currency-input';
import { watch } from 'vue';

const props = defineProps({ modelValue: Number });

const { inputRef, formattedValue, numberValue, setValue } = useCurrencyInput({
  locale: "pt-BR",
  currency: "BRL",
  precision: 2,
  hideCurrencySymbolOnFocus: true,
  hideGroupingSeparatorOnFocus: true,
  hideNegligibleDecimalDigitsOnFocus: true,
  autoDecimalDigits: false,
  useGrouping: true,
  accountingSign: false
});

watch(
  () => props.modelValue,
  (value) => {
    setValue(value);
  }
);
</script>

<template>
  <VTextField
    @focus="$event.target.select()"
    v-model="formattedValue"
    ref="inputRef"
  >
  </VTextField>
</template>
