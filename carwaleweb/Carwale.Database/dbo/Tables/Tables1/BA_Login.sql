CREATE TABLE [dbo].[BA_Login] (
    [ID]               INT          IDENTITY (1, 1) NOT NULL,
    [BrokerID]         INT          NOT NULL,
    [UserName]         VARCHAR (50) NULL,
    [Password]         VARCHAR (50) NULL,
    [Salt]             VARCHAR (50) NULL,
    [IsFirstTime]      BIT          CONSTRAINT [DF_BA.Login_IsFirstTime] DEFAULT ((0)) NOT NULL,
    [IsActive]         BIT          CONSTRAINT [DF_BA.Login_IsActive] DEFAULT ((0)) NOT NULL,
    [IsVersionUpdated] BIT          CONSTRAINT [DF_BA_Login_IsVersionUpdated] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_BA.Login] PRIMARY KEY CLUSTERED ([ID] ASC)
);

