[Sales Invoice Master]

id
customer_id - reference to vendor
gross amount
discount
net amount
created_on
invoice_date
created by - reference to user
FinancialYear - reference
TenantId - reference

[SaleInvoiceDetails]

id
salesinvoice_id
product_id
sale_rate
quantity
created_on
created by - reference to user
discount
FinancialYear - reference
TenantId - reference

CREATE TABLE InvoiceMaster(
	Id int PRIMARY KEY Identity(1,1),
	invoice_no varchar(7) not null,
	customer_id int FOREIGN KEY REFERENCES Vendor(Id),
	gross_amount float not null,
	discount float not null,
	net_amount float not null,
	created_on datetime default CURRENT_TIMESTAMP,
	created_by int FOREIGN KEY REFERENCES [dbo].[User](UserId),
	financial_year int FOREIGN KEY REFERENCES FinancialYear(Id),
	tenant_id int FOREIGN KEY REFERENCES Tenant(Id)
)

CREATE TABLE InvoiceDetails(
	Id int PRIMARY KEY IDENTITY(1,1),
	invoice_id int FOREIGN KEY REFERENCES InvoiceMaster(Id),
	product_id int FOREIGN KEY REFERENCES Product(Id),
	sale_rate float not null,
	quantity int not null,
	created_on datetime default CURRENT_TIMESTAMP,
	created_by int FOREIGN KEY REFERENCES [dbo].[User](UserId),
	discount float not null,
	financial_year int FOREIGN KEY REFERENCES FinancialYear(Id),
	tenant_id int FOREIGN KEY REFERENCES Tenant(Id)
)