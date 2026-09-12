namespace NXAI.Infra.EventBus;

public interface IEventPublisher
{
    /// <summary>
    /// 异步发布对象消息。
    /// </summary>
    /// <typeparam name="T">消息内容对象的类型。</typeparam>
    /// <param name="contentObj">将被序列化的消息体内容。（可为 null）</param>
    /// <param name="callbackName">回调订阅者名称。</param>
    /// <param name="cancellationToken">取消令牌。</param>
    /// <returns>表示异步操作的任务。</returns>
    Task PublishAsync<T>(T contentObj, string? callbackName = null, CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// 异步发布带自定义标头的对象消息。
    /// </summary>
    /// <typeparam name="T">消息内容对象的类型。</typeparam>
    /// <param name="contentObj">将被序列化的消息体内容。（可为 null）</param>
    /// <param name="headers">消息附加标头。</param>
    /// <param name="cancellationToken">取消令牌。</param>
    /// <returns>表示异步操作的任务。</returns>
    Task PublishAsync<T>(T contentObj, IDictionary<string, string?> headers, CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// 发布对象消息。
    /// </summary>
    /// <typeparam name="T">消息内容对象的类型。</typeparam>
    /// <param name="contentObj">将被序列化的消息体内容。（可为 null）</param>
    /// <param name="callbackName">回调订阅者名称。</param>
    void Publish<T>(T contentObj, string? callbackName = null) where T : class;

    /// <summary>
    /// 发布带自定义标头的对象消息。
    /// </summary>
    /// <typeparam name="T">消息内容对象的类型。</typeparam>
    /// <param name="contentObj">将被序列化的消息体内容。（可为 null）</param>
    /// <param name="headers">消息附加标头。</param>
    void Publish<T>(T contentObj, IDictionary<string, string?> headers) where T : class;

    /// <summary>
    /// 异步调度一条消息，在将来指定时间后发布（带标头）。
    /// </summary>
    /// <typeparam name="T">消息内容对象的类型。</typeparam>
    /// <param name="delayTime">消息发布的延迟时间。</param>
    /// <param name="contentObj">将被序列化的消息体内容。（可为 null）</param>
    /// <param name="headers">消息附加标头。</param>
    /// <param name="cancellationToken">取消令牌。</param>
    /// <returns>表示异步操作的任务。</returns>
    Task PublishDelayAsync<T>(TimeSpan delayTime, T? contentObj, IDictionary<string, string?> headers, CancellationToken cancellationToken = default);

    /// <summary>
    /// 异步调度一条消息，在将来指定时间后发布。
    /// </summary>
    /// <typeparam name="T">消息内容对象的类型。</typeparam>
    /// <param name="delayTime">消息发布的延迟时间。</param>
    /// <param name="contentObj">将被序列化的消息体内容。（可为 null）</param>
    /// <param name="callbackName">回调订阅者名称。</param>
    /// <param name="cancellationToken">取消令牌。</param>
    /// <returns>表示异步操作的任务。</returns>
    Task PublishDelayAsync<T>(TimeSpan delayTime, T? contentObj, string? callbackName = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// 调度一条消息，在将来指定时间后发布（带标头）。
    /// </summary>
    /// <typeparam name="T">消息内容对象的类型。</typeparam>
    /// <param name="delayTime">消息发布的延迟时间。</param>
    /// <param name="contentObj">将被序列化的消息体内容。（可为 null）</param>
    /// <param name="headers">消息附加标头。</param>
    void PublishDelay<T>(TimeSpan delayTime, T? contentObj, IDictionary<string, string?> headers);

    /// <summary>
    /// 调度一条消息，在将来指定时间后发布。
    /// </summary>
    /// <typeparam name="T">消息内容对象的类型。</typeparam>
    /// <param name="delayTime">消息发布的延迟时间。</param>
    /// <param name="contentObj">将被序列化的消息体内容。（可为 null）</param>
    /// <param name="callbackName">回调订阅者名称。</param>
    void PublishDelay<T>(TimeSpan delayTime, T? contentObj, string? callbackName = null);

    /// <summary>
    /// 异步发布对象消息。
    /// </summary>
    /// <typeparam name="T">消息内容对象的类型。</typeparam>
    /// <param name="name">主题名称或交换机路由键。</param>
    /// <param name="contentObj">将被序列化的消息体内容。（可为 null）</param>
    /// <param name="callbackName">回调订阅者名称。</param>
    /// <param name="cancellationToken">取消令牌。</param>
    /// <returns>表示异步操作的任务。</returns>
    Task PublishAsync<T>(string name, T? contentObj, string? callbackName = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 异步发布带自定义标头的对象消息。
    /// </summary>
    /// <typeparam name="T">消息内容对象的类型。</typeparam>
    /// <param name="name">主题名称或交换机路由键。</param>
    /// <param name="contentObj">将被序列化的消息体内容。（可为 null）</param>
    /// <param name="headers">消息附加标头。</param>
    /// <param name="cancellationToken">取消令牌。</param>
    /// <returns>表示异步操作的任务。</returns>
    Task PublishAsync<T>(string name, T? contentObj, IDictionary<string, string?> headers,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 发布对象消息。
    /// </summary>
    /// <typeparam name="T">消息内容对象的类型。</typeparam>
    /// <param name="name">主题名称或交换机路由键。</param>
    /// <param name="contentObj">将被序列化的消息体内容。（可为 null）</param>
    /// <param name="callbackName">回调订阅者名称。</param>
    void Publish<T>(string name, T? contentObj, string? callbackName = null);

    /// <summary>
    /// 发布带自定义标头的对象消息。
    /// </summary>
    /// <typeparam name="T">消息内容对象的类型。</typeparam>
    /// <param name="name">主题名称或交换机路由键。</param>
    /// <param name="contentObj">将被序列化的消息体内容。（可为 null）</param>
    /// <param name="headers">消息附加标头。</param>
    void Publish<T>(string name, T? contentObj, IDictionary<string, string?> headers);

    /// <summary>
    /// 异步调度一条消息，在将来指定时间后发布（带标头）。
    /// </summary>
    /// <typeparam name="T">消息内容对象的类型。</typeparam>
    /// <param name="delayTime">消息发布的延迟时间。</param>
    /// <param name="name">主题名称或交换机路由键。</param>
    /// <param name="contentObj">将被序列化的消息体内容。（可为 null）</param>
    /// <param name="headers">消息附加标头。</param>
    /// <param name="cancellationToken">取消令牌。</param>
    /// <returns>表示异步操作的任务。</returns>
    Task PublishDelayAsync<T>(TimeSpan delayTime, string name, T? contentObj, IDictionary<string, string?> headers, CancellationToken cancellationToken = default);

    /// <summary>
    /// 异步调度一条消息，在将来指定时间后发布。
    /// </summary>
    /// <typeparam name="T">消息内容对象的类型。</typeparam>
    /// <param name="delayTime">消息发布的延迟时间。</param>
    /// <param name="name">主题名称或交换机路由键。</param>
    /// <param name="contentObj">将被序列化的消息体内容。（可为 null）</param>
    /// <param name="callbackName">回调订阅者名称。</param>
    /// <param name="cancellationToken">取消令牌。</param>
    /// <returns>表示异步操作的任务。</returns>
    Task PublishDelayAsync<T>(TimeSpan delayTime, string name, T? contentObj, string? callbackName = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// 调度一条消息，在将来指定时间后发布（带标头）。
    /// </summary>
    /// <typeparam name="T">消息内容对象的类型。</typeparam>
    /// <param name="delayTime">消息发布的延迟时间。</param>
    /// <param name="name">主题名称或交换机路由键。</param>
    /// <param name="contentObj">将被序列化的消息体内容。（可为 null）</param>
    /// <param name="headers">消息附加标头。</param>
    void PublishDelay<T>(TimeSpan delayTime, string name, T? contentObj, IDictionary<string, string?> headers);

    /// <summary>
    /// 调度一条消息，在将来指定时间后发布。
    /// </summary>
    /// <typeparam name="T">消息内容对象的类型。</typeparam>
    /// <param name="delayTime">消息发布的延迟时间。</param>
    /// <param name="name">主题名称或交换机路由键。</param>
    /// <param name="contentObj">将被序列化的消息体内容。（可为 null）</param>
    /// <param name="callbackName">回调订阅者名称。</param>
    void PublishDelay<T>(TimeSpan delayTime, string name, T? contentObj, string? callbackName = null);
}
