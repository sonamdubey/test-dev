CREATE TABLE [dbo].[TC_Users] (
    [Id]                  INT                 IDENTITY (1, 1) NOT NULL,
    [BranchId]            NUMERIC (18)        NOT NULL,
    [RoleId]              INT                 NULL,
    [UserName]            VARCHAR (150)       NULL,
    [Email]               VARCHAR (100)       NOT NULL,
    [Password]            VARCHAR (20)        NULL,
    [Mobile]              VARCHAR (15)        NULL,
    [EntryDate]           DATETIME            CONSTRAINT [DF_TC_Users_EntryDate] DEFAULT (getdate()) NOT NULL,
    [DOB]                 DATE                NULL,
    [DOJ]                 DATE                NULL,
    [Sex]                 VARCHAR (6)         NULL,
    [Address]             VARCHAR (500)       NULL,
    [IsActive]            BIT                 CONSTRAINT [DF_TC_Users_IsActive] DEFAULT ((1)) NOT NULL,
    [ModifiedBy]          INT                 NULL,
    [ModifiedDate]        DATETIME            NULL,
    [IsFirstTimeLoggedIn] BIT                 CONSTRAINT [DF__TC_Users__IsFirs__0282E087] DEFAULT ((1)) NULL,
    [IsCarwaleUser]       BIT                 CONSTRAINT [DF__TC_Users__IsCarw__08C5C9C2] DEFAULT ((0)) NULL,
    [UniqueId]            VARCHAR (50)        CONSTRAINT [DF_TC_Users_UniqueId] DEFAULT (CONVERT([varchar](20),right(newid(),(12)),(0))+CONVERT([varchar](8),getdate(),(112))) NULL,
    [TodaysCallCount]     SMALLINT            CONSTRAINT [DF_TC_Users_TodaysCallCount] DEFAULT ((0)) NULL,
    [HierId]              [sys].[hierarchyid] NULL,
    [lvl]                 AS                  ([HierId].[GetLevel]()) PERSISTED,
    [NodeCode]            AS                  ([HierId].[ToString]()) PERSISTED,
    [GCMRegistrationId]   VARCHAR (250)       NULL,
    [PwdRecoveryEmail]    VARCHAR (50)        NULL,
    [VTONumbers]          VARCHAR (50)        NULL,
    [BranchLocationId]    INT                 NULL,
    [CityId]              INT                 NULL,
    [AreaId]              INT                 NULL,
    [IsAgency]            BIT                 NULL,
    [AgencyType]          TINYINT             NULL,
    [ReportingTo]         INT                 NULL,
    [LoginType]           TINYINT             NULL,
    [LoginTime]           DATETIME            NULL,
    [UUIDIOS]             VARCHAR (200)       NULL,
    [DeviceTokenIOS]      VARCHAR (70)        NULL,
    [IsShownCarwale]      BIT                 NULL,
    [ImageUrl]            VARCHAR (400)       NULL,
    [HashSalt]            VARCHAR (10)        NULL,
    [PasswordHash]        VARCHAR (100)       NULL,
    CONSTRAINT [PK_TC_Users] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ__TC_Users__A9D1053425B3E0CF] UNIQUE NONCLUSTERED ([Email] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_TC_Users_BranchId]
    ON [dbo].[TC_Users]([BranchId] ASC, [IsActive] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_TC_Users_UniqueId]
    ON [dbo].[TC_Users]([UniqueId] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_TC_Users_BranchId_IsActive]
    ON [dbo].[TC_Users]([BranchId] ASC, [IsActive] ASC, [IsCarwaleUser] ASC);


GO
-- =============================================
-- Author:		Ruchira Patil
-- Create date: 23rd Oct 2015
-- Description:	To update the mapped surveyor's login id in TC_DealerSurveyorMapping when agency updates the surveyor's login id
-- =============================================
CREATE TRIGGER [dbo].[TrigUpdateMobileNo] 
   ON  [dbo].[TC_Users]
   FOR UPDATE
AS 
BEGIN

	DECLARE @TC_UserId INT,@Mobile VARCHAR(15)
	SELECT @TC_UserId = Id,@Mobile = Mobile
	FROM Inserted
	 
	IF UPDATE(Mobile)
	BEGIN
		UPDATE TC_DealerSurveyorMapping SET SurveyorMobileNo = @Mobile,ModifiedDate = GETDATE() WHERE SurveyorId = @TC_UserId
	END
END
 

 -----------------------------------


