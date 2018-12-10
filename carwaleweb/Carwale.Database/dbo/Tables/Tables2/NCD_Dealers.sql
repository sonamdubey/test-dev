CREATE TABLE [dbo].[NCD_Dealers] (
    [DealerId]     INT           NOT NULL,
    [UserId]       VARCHAR (50)  NOT NULL,
    [Password]     VARCHAR (50)  NOT NULL,
    [JoiningDate]  DATETIME      NOT NULL,
    [IsActive]     BIT           CONSTRAINT [DF_NCD_Dealers_IsActive] DEFAULT ((1)) NOT NULL,
    [NCD_Website]  VARCHAR (100) NULL,
    [IsPanelOnly]  BIT           CONSTRAINT [DF_NCD_Dealers_IsPanelOnly] DEFAULT ((0)) NOT NULL,
    [Longitude]    VARCHAR (20)  NULL,
    [Lattitude]    VARCHAR (20)  NULL,
    [IsPremium]    BIT           NULL,
    [IsMVL]        BIT           CONSTRAINT [DF_NCD_Dealers_IsMVL] DEFAULT ((0)) NULL,
    [TCDealerId]   NUMERIC (18)  NULL,
    [CampaignId]   NUMERIC (18)  NULL,
    [TargetLeads]  INT           NULL,
    [DelLeads]     INT           NULL,
    [IsDailyBased] BIT           NULL,
    [DailyDel]     INT           NULL,
    [DailyCount]   INT           NULL,
    CONSTRAINT [PK_NCD_Dealers] PRIMARY KEY CLUSTERED ([DealerId] ASC)
);


GO

CREATE TRIGGER [dbo].[NCD_UserInsert]
ON [dbo].[NCD_Dealers]
FOR INSERT
AS
Begin

	-- New User (dealer) creation in  NCD_users table for this new dealer
    Insert into NCD_Users (DealerId, RoleId,UserName,Email,Password,Mobile,EntryDate,Address,isactive) 
	select top 1 ND.DealerId,'1' as RoleId,DNC.Name,ND.UserId,ND.Password,
    DNC.ContactNo,getdate(),DNC.address+' -'+DNC.Pincode as Address,'1' as isactive
    from Inserted ND join Dealer_NewCar DNC on DNC.Id=ND.DealerId
	order by ND.dealerid desc
	
	-- Role defining in NCD_Roles table for this new dealer
	
	Insert Into NCD_Roles (RoleName,RoleDescription,TaskSet,RoleCreationDate,DealerId,IsActive)	
	Select top 1 'Super Admin' as RoleName,'Super Admin' as RoleDescription,'1,2,4,5,6' as TaskSet,
	Getdate(),DealerId,0 from Inserted order by DealerId desc
	
End

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Flag will be true if dealer only need NCD admin panel to manage leads, false in case dealer has micro site', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NCD_Dealers', @level2type = N'COLUMN', @level2name = N'IsPanelOnly';

