﻿Feature: Salesflow
	Tests for the lead/opportunity entity to show special dialogs like the qualify lead, process flows and related entities

@API @Chrome @Cleanup @Sales
Scenario: Lead Qualification
Given a lead named ToQualify with the following values
	| Property     | Value              |
	| First Name   | Qualify            |
	| Last Name    | Test               |
	| Topic        | Qualification Test |
	| Company Name | Qualify Account    |
When ToQualify is qualified to a
	| Account | Opportunity | Contact |
	| true    | true        | true    |
Then an account named QualifyAccount exists with the following values
	| Property         | Value           |
	| Originating Lead | ToQualify       |
	| Account Name     | Qualify Account |
And a contact exists with the following values
	| Property         | Value          |
	| Originating Lead | ToQualify      |
	| Company Name     | QualifyAccount |
	| First Name       | Qualify        |
	| Last Name        | Test           |
And an opportunity exists with the following values
	| Property         | Value              |
	| Originating Lead | ToQualify          |
	| Topic            | Qualification Test |

@API @Chrome @Cleanup
Scenario: Convert Quote to Sales Order
Given an account named QuoteTesting with the following values
	| Property              | Value                   |
	| Account Name          | QuoteTesting            |
And a quote named TestQuote with the following values
	| Property           | Value                |
	| Name               | AT Quote             |
	| Price List         | Automated Testing PL |
	| Potential Customer | QuoteTesting         |

When TestQuote is activated and converted to a sales order named TestOrder
Then TestOrder has the following values
	| Property   | Value                |
	| Name       | AT Quote             |
	| Price List | Automated Testing PL |
	| Customer   | QuoteTesting         |
	| Quote      | TestQuote            |
