CREATE TABLE [dbo].[DCRM_DisEngagedDealers] (
    [Id]                          INT          IDENTITY (1, 1) NOT NULL,
    [DealerID]                    NUMERIC (18) NOT NULL,
    [IsGetResponse]               BIT          NOT NULL,
    [IsCarUploaded]               BIT          NOT NULL,
    [IsSuccessfullFieldVisit]     BIT          NOT NULL,
    [IsCallConnected]             BIT          NOT NULL,
    [IsTillDayRenewal]            BIT          NULL,
    [IsTillDayToNextMonthRenewal] BIT          NULL,
    [IsLastMonthRenewal]          BIT          NULL,
    CONSTRAINT [PK_DCRM_DisEngagedDealers] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Is get response on car since last seven days', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DCRM_DisEngagedDealers', @level2type = N'COLUMN', @level2name = N'IsGetResponse';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Is car uploaded by dealer since last 7 days', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DCRM_DisEngagedDealers', @level2type = N'COLUMN', @level2name = N'IsCarUploaded';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Is there meeting of descison maker since last 15 visits', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DCRM_DisEngagedDealers', @level2type = N'COLUMN', @level2name = N'IsSuccessfullFieldVisit';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Is call connected with the dealer since last 15 days', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DCRM_DisEngagedDealers', @level2type = N'COLUMN', @level2name = N'IsCallConnected';

