# Ronathan File Crawl Client
The Ronathan file crawling upload client agent will send files for OSCAL intake with risk/compliance assessment. Option 1 is an example of how to upload a single file to the Ronathan web hook. Options 2-6 will crawl various network scopes looking for file to send back for analalysis.

## Ronathan Automated Compliance & Risk Analysis
Compliance and risk assesment are based on standard risk management frameworks and globally-accessible knowledge base of adversary tactics and techniques based on real-world observations.

## Getting Started
1) Change the email and password in the getAccessToken function (Helper.cs line: 74,75) to a licensed email and password.
2) Change the subscribeToWebhookAsync function in the Program.cs file to have the same licensed email from step one and location of a listener for ronathan to send the analysis to (Program.cs line: 213, 215)
3) Run the program
