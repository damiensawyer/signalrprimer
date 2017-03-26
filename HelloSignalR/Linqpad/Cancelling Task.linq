<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task Main()
{
	var cts = new CancellationTokenSource();
	await Task.Factory.StartNew(async () =>
			{
				var i = 0;
				while (true)
				{
					i++.Dump();
					await Task.Delay(300);
				}
			}, cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current);

	await Task.Run(async () => {
		await Task.Delay(1000);
		"completing".Dump("ctrl-shift-f5 to kill threads if it's still running.");
		cts.Cancel();
	});
}
