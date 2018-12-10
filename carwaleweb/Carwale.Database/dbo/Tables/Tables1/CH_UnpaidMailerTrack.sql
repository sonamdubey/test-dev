CREATE TABLE [dbo].[CH_UnpaidMailerTrack] (
    [Id]            NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [InquiryId]     NUMERIC (18) NOT NULL,
    [CustomerId]    NUMERIC (18) NOT NULL,
    [MailDate]      DATETIME     CONSTRAINT [DF_CH_UnpaidMailerTrack_MailDate] DEFAULT (getdate()) NOT NULL,
    [ClickDate]     DATETIME     NULL,
    [IsClicked]     BIT          CONSTRAINT [DF_CH_UnpaidMailerTrack_IsClicked] DEFAULT ((0)) NULL,
    [MailerDay]     INT          NOT NULL,
    [MailerTypesId] SMALLINT     CONSTRAINT [DF_CH_UnpaidMailerTrack_MailerTypesId] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_CH_UnpaidMailerTrack] PRIMARY KEY CLUSTERED ([Id] ASC)
);

