using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Dispatching;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StreamingTokenTest;

public partial class MainPageViewModel : ObservableObject
{
    private const string LOREM = """
Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Nibh praesent tristique magna sit amet purus gravida. Turpis massa sed elementum tempus egestas sed sed risus pretium. Morbi tristique senectus et netus et malesuada fames ac turpis. Consectetur adipiscing elit duis tristique sollicitudin nibh sit. Tristique risus nec feugiat in fermentum posuere urna nec tincidunt. Sit amet consectetur adipiscing elit pellentesque habitant morbi tristique senectus. Cursus sit amet dictum sit amet. In ornare quam viverra orci. Aliquam eleifend mi in nulla posuere sollicitudin aliquam. Enim nulla aliquet porttitor lacus luctus accumsan tortor posuere. Pulvinar mattis nunc sed blandit libero.

Egestas dui id ornare arcu. Maecenas pharetra convallis posuere morbi leo urna. A cras semper auctor neque vitae tempus quam. Mauris rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar. Eu tincidunt tortor aliquam nulla facilisi. Nisl condimentum id venenatis a. Suscipit tellus mauris a diam maecenas sed enim ut. Gravida cum sociis natoque penatibus et magnis dis. Montes nascetur ridiculus mus mauris vitae. Adipiscing commodo elit at imperdiet.

Ultricies leo integer malesuada nunc vel risus commodo viverra. Ipsum nunc aliquet bibendum enim facilisis gravida neque convallis. Aenean sed adipiscing diam donec. Convallis tellus id interdum velit laoreet id donec. Id velit ut tortor pretium viverra suspendisse. Consectetur purus ut faucibus pulvinar elementum integer. Auctor elit sed vulputate mi sit amet. Etiam tempor orci eu lobortis elementum nibh. Commodo elit at imperdiet dui. Senectus et netus et malesuada.
""";

    private readonly DispatcherQueue dispatcher;

    [ObservableProperty]
    public bool showPlaceholder = false;

    [ObservableProperty]
    public string outputText = string.Empty;

    [RelayCommand]
    public async Task GenerateOutput()
    {
        ShowPlaceholder = true;
        OutputText = string.Empty;

        var fullResponse = await Task.Run(() => StreamResponse((output) =>
        {
            dispatcher.TryEnqueue(() =>
            {
                ShowPlaceholder = false;
                OutputText = output;
            });
        }));

        ShowPlaceholder = false;
        OutputText = fullResponse;
    }

    public MainPageViewModel()
    {
        dispatcher = DispatcherQueue.GetForCurrentThread();
    }

    private async Task<string> StreamResponse(Action<string> onToken)
    {
        for (var i = 0; i < LOREM.Length; i = Math.Min(i + 3, LOREM.Length))
        {
            var subString = await Task.Run(() =>
            {
                return LOREM.Substring(0, i);
            });
            onToken(subString);
        }

        return LOREM;
    }

    //private Task<string> StreamResponse(Action<string> onToken)
    //{
    //    for (var i = 0; i < LOREM.Length; i = Math.Min(i + 3, LOREM.Length))
    //    {
    //        var tcs = new TaskCompletionSource<string>();
    //        var thread = new Thread(() =>
    //        {
    //            var subString = LOREM.Substring(0, i);
    //            tcs.SetResult(subString);
    //            return;
    //        });
    //        thread.Start();

    //        var subString = tcs.Task.Result;

    //        thread.Join();

    //        onToken(subString);
    //    }

    //    return Task.FromResult(LOREM);
    //}
}
