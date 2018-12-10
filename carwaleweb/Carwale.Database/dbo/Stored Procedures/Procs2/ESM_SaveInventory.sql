IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ESM_SaveInventory]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ESM_SaveInventory]
GO

	
-- =============================================
-- Author:		Amit Yadav(7th Oct 2015)
-- Description:	To Save ESM_Inventory
-- =============================================
CREATE PROCEDURE [dbo].[ESM_SaveInventory]
	@Id INT,
	@ProposalId INT,
	@AdFor VARCHAR(30),
	@TargetedCar INT,
	@Placement INT,
	@AdUnit INT,
	@IsApp  AS BIT,
	@IsDesktop  AS BIT,
	@IsMobile  AS BIT,
	@StartDate AS DATETIME,
	@EndDate AS DATETIME,
	@Comment VARCHAR(250),
	@UpdatedBy INT,
	@LastUpdatedOn AS DATETIME,
	@RetVal AS INT OUT

AS
BEGIN
	--Avoid Duplicate Entry
	 SET @RetVal= -1
	 SELECT * FROM ESM_Inventory AS ECI WITH(NOLOCK) WHERE ECI.AdFor=@AdFor AND ECI.TargetedCar=@TargetedCar AND ECI.Placement=@Placement AND ECI.AdUnit=@AdUnit AND ECI.Id=@Id
	 IF @@ROWCOUNT = 0 AND @ID=-1

		 BEGIN
				INSERT INTO ESM_Inventory(ProposalId,AdFor,TargetedCar,Placement,AdUnit,Comment,UpdatedBy,LastUpdatedOn)
				VALUES( @ProposalId,@AdFor,@TargetedCar,@Placement,@AdUnit,@Comment,@UpdatedBy,@LastUpdatedOn)
			   
				SET @RetVal = SCOPE_IDENTITY()
				SET @Id = @@IDENTITY
				WHILE(@StartDate<=@EndDate)
				BEGIN
				 INSERT INTO ESM_InventoryBooking(InventoryId,InventoryDate,isActive)VALUES(@RetVal,@StartDate,1)
				 SELECT @StartDate = DATEADD(DAY, 1, @StartDate )
				 END
		 END
		 
	 ELSE

		BEGIN
		SELECT * FROM ESM_Inventory AS ECI WITH(NOLOCK) WHERE ECI.AdFor=@AdFor AND ECI.TargetedCar=@TargetedCar AND ECI.Placement=@Placement AND ECI.AdUnit=@AdUnit AND ECI.Id<>@Id
		IF @@ROWCOUNT = 0
		BEGIN
				UPDATE ESM_Inventory SET TargetedCar=@TargetedCar,Placement=@Placement, AdUnit=@AdUnit, UpdatedBy=@UpdatedBy ,LastUpdatedOn=@LastUpdatedOn
				WHERE  Id=@ID
			
				SET @RetVal = @ID
		 END
	END
END

