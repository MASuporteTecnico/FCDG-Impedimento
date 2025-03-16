<template>
  <ejs-richtexteditor id="default" ref="rteInstance" v-model="modelValue" :iframeSettings="iframeData" :toolbarSettings="toolbarSettings" :enableResize="true" :enableTabKey="true" :readonly="props.ReadOnly"></ejs-richtexteditor>
</template>

<script setup>
const props = defineProps({
  ReadOnly: { type: Boolean, default: false },
});

import { provide } from "vue";
import { RichTextEditorComponent as EjsRichtexteditor, Toolbar, Link, Image, Table, HtmlEditor, QuickToolbar, Resize, PasteCleanup, FormatPainter, EmojiPicker, readOnlyMode } from "@syncfusion/ej2-vue-richtexteditor";

const modelValue = defineModel();
const rteInstance = ref(null);
const richtexteditor = [Toolbar, Link, Image, Table, HtmlEditor, QuickToolbar, Resize, PasteCleanup, FormatPainter, EmojiPicker];

const iframeData = {
  enable: true,
};

const toolbarSettings = {
  type: "MultiRow",
  enable: !props.ReadOnly,
  items: ["Bold", "Italic", "Underline", "StrikeThrough", "FontName", "FontSize", "FontColor", "BackgroundColor", "LowerCase", "UpperCase", "SubScript", "SuperScript", "|", "Formats", "Alignments", "OrderedList", "UnorderedList", "NumberFormatList", "BulletFormatList", "Outdent", "Indent", "|", "CreateLink", "Image", "|", "ClearFormat", "Print", "SourceCode", "FullScreen", "|", "Undo", "Redo"],
};

provide("richtexteditor", richtexteditor);


</script>

<style>
    @import "@syncfusion/ej2-base/styles/bootstrap5.css";
    @import "@syncfusion/ej2-inputs/styles/bootstrap5.css";
    @import "@syncfusion/ej2-lists/styles/bootstrap5.css";
    @import "@syncfusion/ej2-popups/styles/bootstrap5.css";
    @import "@syncfusion/ej2-buttons/styles/bootstrap5.css";
    @import "@syncfusion/ej2-navigations/styles/bootstrap5.css";
    @import "@syncfusion/ej2-splitbuttons/styles/bootstrap5.css";
    @import "@syncfusion/ej2-vue-richtexteditor/styles/bootstrap5.css";
</style>
