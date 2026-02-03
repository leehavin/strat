import { dotnet } from './_framework/dotnet.js'

const is_browser = typeof window != "undefined";
if (!is_browser) throw new Error(`Expected to be running in a browser`);

const dotnetRuntime = await dotnet
    .withDiagnosticTracing(false)
    .withApplicationArgumentsFromQuery()
    .withConfig({
        // 配置资源加载
        maxParallelDownloads: 10,
        enableDownloadRetry: true
    })
    .create();

const config = dotnetRuntime.getConfig();

// 预加载 SkiaSharp 需要的资源
try {
    await dotnetRuntime.runMain(config.mainAssemblyName, [globalThis.location.href]);
} catch (error) {
    console.error('Application startup failed:', error);
    document.getElementById('out').innerHTML = `
        <div style="padding: 20px; color: red;">
            <h3>启动失败</h3>
            <p>${error.message}</p>
            <p>请尝试刷新页面或检查浏览器控制台获取详细信息。</p>
        </div>
    `;
}
