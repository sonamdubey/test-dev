CREATE TABLE [dbo].[MM_AvailableNumbersLog] (
    [Id]                INT          IDENTITY (1, 1) NOT NULL,
    [MaskingNumber]     VARCHAR (50) NOT NULL,
    [StateId]           SMALLINT     NOT NULL,
    [CityId]            SMALLINT     NOT NULL,
    [ServiceProviderId] SMALLINT     NOT NULL,
    [IsValid]           BIT          CONSTRAINT [DF_MM_AvailableNumbersLog_IsValid] DEFAULT ((1)) NOT NULL,
    [UpdatedBy]         INT          NOT NULL,
    [CreatedOn]         DATETIME     CONSTRAINT [DF_MM_AvailableNumbersLog_CreatedOn] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_MM_AvailableNumbersLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

