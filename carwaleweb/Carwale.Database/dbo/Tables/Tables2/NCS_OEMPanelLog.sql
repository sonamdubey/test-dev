CREATE TABLE [dbo].[NCS_OEMPanelLog] (
    [Id]        NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Type]      SMALLINT     NOT NULL,
    [UserId]    NUMERIC (18) NOT NULL,
    [Logintime] DATETIME     NOT NULL,
    [IPAddres]  VARCHAR (50) NULL,
    CONSTRAINT [PK_NCS_OEMPanelLog] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'0-RM Panel, 1-Skoda, 2-GM, 3-Mahindra, 4-Mahindra Eenault, Dealers-5,Honda-6, Hyundai-7,Maruti-8', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NCS_OEMPanelLog', @level2type = N'COLUMN', @level2name = N'Type';

