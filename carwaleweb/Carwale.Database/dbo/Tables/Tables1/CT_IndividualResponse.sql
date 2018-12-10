CREATE TABLE [dbo].[CT_IndividualResponse] (
    [ID]             INT           IDENTITY (1, 1) NOT NULL,
    [BuyerId]        INT           NULL,
    [BuyerName]      VARCHAR (50)  NULL,
    [BuyerMobile]    VARCHAR (20)  NULL,
    [BuyerEmail]     VARCHAR (100) NULL,
    [CarMakeName]    VARCHAR (30)  NULL,
    [CarModelName]   VARCHAR (30)  NULL,
    [CarVersionName] VARCHAR (100) NULL,
    [CarMakeYear]    DATETIME      NULL,
    [CarColor]       VARCHAR (100) NULL,
    [CarFuelType]    VARCHAR (20)  NULL,
    [CarPrice]       DECIMAL (18)  NULL,
    [CTStatusId]     TINYINT       NULL,
    [EntryDate]      DATETIME      CONSTRAINT [DF_CT_IndividualResponse_EntryDate] DEFAULT (getdate()) NULL,
    [CityName]       VARCHAR (50)  NULL
);


GO
CREATE CLUSTERED INDEX [IX_CT_IndividualResponse_ID]
    ON [dbo].[CT_IndividualResponse]([ID] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_CT_Indivi_CTStatusId]
    ON [dbo].[CT_IndividualResponse]([CTStatusId] ASC);

