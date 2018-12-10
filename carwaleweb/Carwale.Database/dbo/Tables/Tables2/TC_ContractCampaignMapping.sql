CREATE TABLE [dbo].[TC_ContractCampaignMapping] (
    [Id]                    INT      IDENTITY (1, 1) NOT NULL,
    [ContractId]            INT      NULL,
    [CampaignId]            INT      NULL,
    [DealerId]              BIGINT   NULL,
    [StartDate]             DATETIME NULL,
    [EndDate]               DATETIME NULL,
    [TotalGoal]             INT      NULL,
    [TotalDelivered]        INT      NULL,
    [ContractStatus]        INT      NULL,
    [CreatedOn]             DATETIME CONSTRAINT [DF_TC_ContractCampaignMapping_CreatedOn] DEFAULT (getdate()) NULL,
    [ContractBehaviour]     SMALLINT NULL,
    [CostPerLead]           INT      NULL,
    [ContractType]          INT      NULL,
    [ReplacementContractId] INT      NULL,
    [ActionTakenBy]         INT      NULL,
    [IsActiveContract]      BIT      NULL,
    [AutoPauseDate]         DATETIME NULL,
    [ApplicationID]         TINYINT  DEFAULT ((1)) NULL,
    CONSTRAINT [PK__TC_Contr__3214EC0705BDA937] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_TC_ContractCampaignMapping_DealerId]
    ON [dbo].[TC_ContractCampaignMapping]([DealerId] ASC, [CampaignId] ASC, [ContractId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_ContractCampaignMapping_ContractId]
    ON [dbo].[TC_ContractCampaignMapping]([ContractId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_ContractCampaignMapping_StartDate_EndDate]
    ON [dbo].[TC_ContractCampaignMapping]([StartDate] ASC, [EndDate] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_ContractCampaignMapping_CampaignId]
    ON [dbo].[TC_ContractCampaignMapping]([CampaignId] ASC);


GO

-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <12 OCT 15>
-- Description:	<keep log on contract status changes>
-- =============================================
CREATE TRIGGER [dbo].[TrigUpdateContractCampaignMapping]
   ON  [dbo].[TC_ContractCampaignMapping]
   FOR  UPDATE
AS 
BEGIN

	IF(UPDATE(ContractStatus))
		BEGIN
			INSERT INTO TC_ContractStatusLog(TC_ContractCampaignMappingId,PreContractStatus,CurrentContractStatus,ActionTakenBy,ActionTakenOn)
			SELECT  I.ID ,D.ContractStatus,I.ContractStatus,I.ActionTakenBy ,GETDATE()
			FROM inserted I WITH(NOLOCK)
			INNER JOIN deleted D WITH(NOLOCK) ON I.Id = D.Id
		END
END


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-Active, 2- Paused, 3-Closed, 4-Aborted', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TC_ContractCampaignMapping', @level2type = N'COLUMN', @level2name = N'ContractStatus';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 for lead based 2 for date based', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TC_ContractCampaignMapping', @level2type = N'COLUMN', @level2name = N'ContractBehaviour';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 for normal lead 2 for replacement lead', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TC_ContractCampaignMapping', @level2type = N'COLUMN', @level2name = N'ContractType';

