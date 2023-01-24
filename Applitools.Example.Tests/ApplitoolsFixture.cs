using Xunit;

namespace Applitools.Example.Tests;

/// <summary>
/// Fixture for one-time setup and cleanup
/// </summary>
public class ApplitoolsFixture : IDisposable
{
  // Test control inputs to read once and share for all tests
  public string? ApplitoolsApiKey;
  public bool Headless;

  // Applitools objects to share for all tests
  public BatchInfo Batch;
  public Configuration Config;
  public ClassicRunner Runner;

  /// <summary>
  /// Sets up the configuration for running visual tests in the Ultrafast Grid.
  /// The configuration is shared by all tests in a test suite, so it belongs in a collection fixture.
  /// If you have more than one test class, then you should abstract this configuration to avoid duplication.
  /// <summary>
  public ApplitoolsFixture()
  {
    // Read the Applitools API key from an environment variable.
    ApplitoolsApiKey = Environment.GetEnvironmentVariable("APPLITOOLS_API_KEY");

    // Read the headless mode setting from an environment variable.
    // Use headless mode for Continuous Integration (CI) execution.
    // Use headed mode for local development.
    Headless = Environment.GetEnvironmentVariable("HEADLESS")?.ToLower() == "true";

    // Create the classic runner.
    Runner = new ClassicRunner();

    // Create a new batch for tests.
    // A batch is the collection of visual checkpoints for a test suite.
    // Batches are displayed in the Eyes Test Manager, so use meaningful names.
    Batch = new BatchInfo("Example: Selenium C# xUnit.net with the Classic runner");

    // Create a configuration for Applitools Eyes.
    Config = new Configuration();

    // Set the Applitools API key so test results are uploaded to your account.
    // If you don't explicitly set the API key with this call,
    // then the SDK will automatically read the `APPLITOOLS_API_KEY` environment variable to fetch it.
    Config.SetApiKey(ApplitoolsApiKey);

    // Set the batch for the config.
    Config.SetBatch(Batch);
  }

  /// <summary>
  /// Prints the final summary report for the test suite.
  /// <summary>
  public void Dispose()
  {
    // Close the batch and report visual differences to the console.
    // Note that it forces xUnit.net to wait synchronously for all visual checkpoints to complete.
    TestResultsSummary allTestResults = Runner.GetAllTestResults();
    Console.WriteLine(allTestResults);
  }
}

[CollectionDefinition("Applitools collection")]
public class ApplitoolsCollection : ICollectionFixture<ApplitoolsFixture> {}