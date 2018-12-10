CREATE TABLE [dbo].[TeleCaller_SMS] (
    [ServiceType] SMALLINT     NOT NULL,
    [Number]      VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_TeleCaller_SMS] PRIMARY KEY CLUSTERED ([ServiceType] ASC, [Number] ASC) WITH (FILLFACTOR = 90)
);

