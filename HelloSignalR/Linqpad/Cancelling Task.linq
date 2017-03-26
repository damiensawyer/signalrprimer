<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task Main()
{
	var cts = new CancellationTokenSource();

	Action<CancellationToken> mainLoop = async (t) =>
	{
		var i = 0;
		try
		{
			while (true)
			{
				t.ThrowIfCancellationRequested();
				i++.Dump();
				await Task.Delay(300);
			}
		}
		catch (OperationCanceledException ex)
		{
			ex.Message.Dump("cancelled");
		}
	};

	await Task.Factory.StartNew(() => {mainLoop(cts.Token);},
		cts.Token, // I'm not sure what passing this achieves
		TaskCreationOptions.LongRunning, 
		TaskScheduler.Default);

	await Task.Run(async () => {
		await Task.Delay(1000);
		"completing".Dump();
		cts.Cancel();
	});
}