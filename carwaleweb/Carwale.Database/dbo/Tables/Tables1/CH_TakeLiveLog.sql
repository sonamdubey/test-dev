CREATE TABLE [dbo].[CH_TakeLiveLog] (
    [Id]        BIGINT   IDENTITY (1, 1) NOT NULL,
    [InquiryId] BIGINT   NOT NULL,
    [UpdatedBy] INT      NOT NULL,
    [UpdatedOn] DATETIME CONSTRAINT [DF_CH_TakeLiveLog_UpdatedOn] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_CH_TakeLiveLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

