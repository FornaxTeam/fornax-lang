const wasmCode = await Deno.readFile(Deno.args[0] + ".wasm");
const wasmModule = new WebAssembly.Module(wasmCode);
const wasmInstance = new WebAssembly.Instance(wasmModule, {
    "console": {
        log: console.log
    }
});
