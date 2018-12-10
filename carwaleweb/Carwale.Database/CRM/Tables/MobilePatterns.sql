CREATE TABLE [CRM].[MobilePatterns] (
    [Number] VARCHAR (50)  NOT NULL,
    [SP]     VARCHAR (50)  NULL,
    [State]  VARCHAR (100) NULL,
    [Region] VARCHAR (100) NULL,
    CONSTRAINT [PK_MobilePatterns] PRIMARY KEY CLUSTERED ([Number] ASC) WITH (FILLFACTOR = 90)
);

