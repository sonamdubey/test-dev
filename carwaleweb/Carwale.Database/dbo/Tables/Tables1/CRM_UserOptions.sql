CREATE TABLE [dbo].[CRM_UserOptions] (
    [UserId]               NUMERIC (18) NOT NULL,
    [IsVerificationScript] BIT          CONSTRAINT [DF_OprUserOptions_IsVerificationScript] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_OprUserOptions] PRIMARY KEY CLUSTERED ([UserId] ASC)
);

