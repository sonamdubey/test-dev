IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_InsertRSAPackages]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_InsertRSAPackages]
GO

	-- =============================================
-- Author:		Tejashree Patil
-- Create date: 24-Sep-2014
-- Description:	Insert RSA form details into table TC_SoldRSAPackages.
-- Modified By: Ashwini Dhamankar on Nov 20,2014. Compared Id of TC_AvalilableRSAPackages instead of PackageId and @RegistrationNo and @VersionId.
-- Modified By: Yuga Hatolkar on March 12, 2015. 
-- Description: Added Parameters RegistrationType, ChassisNo, EngineNo, Kilometer, CarFittedWith, CityId, AreaId, Amount, ReqRSAPackageId, Address,  and separated dealer and
--			    individual types.
--  Modified by : Ruchira Patil on 20th Mar 2015 (To add data in Absure_RSAPolicy against the recently added RSA package and To update policy no)
--  exec TC_InsertRSAPackages 5,NULL,NULL,'Shruti','9898989865','ash30@gmail.com',3,'2011-03-25 11:07:56.800',3550,'MH 05 0000033',1,NULL,NULL,5,NULL,1,10,1500,1,NULL,0
-- =============================================
CREATE PROCEDURE [dbo].[TC_InsertRSAPackages] 
(
	@BranchId INT ,
	@UserId INT = NULL,
	@PackageId INT = NULL,
	@Name VARCHAR(100),
	@MobileNo VARCHAR(15),
	@Email VARCHAR(100),	
	@Quantity INT = NULL,
    @MakeYear DATETIME,
	@VersionId INT , 
	@RegistrationNo VARCHAR(20),
	@RegistrationType INT = 1,
	@ChassisNo VARCHAR(50) = NULL,
	@EngineNo VARCHAR(50) = NULL,
	@Kilometer INT = NULL,	
	@CarFittedWith TINYINT = NULL,	
	@CityId BIGINT = NULL,
	@AreaId BIGINT = NULL,
	@Amount FLOAT = NULL,
	@ReqRSAPackageId INT = NULL,
	@Address VARCHAR(500) = NULL,
	@IsDealer BIT = NULL,
	@Id INT = NULL OUTPUT,
	@PolicyNo VARCHAR(50) = NULL OUTPUT
)
AS
BEGIN 
	IF (@RegistrationType = 1)	-- Dealer Type
	BEGIN
		IF (SELECT AvailableQuantity FROM TC_AvailableRSAPackages WITH(NOLOCK) WHERE Id = @PackageId AND BranchId=@BranchId) > 0   --Modified By: Ashwini Dhamankar on Nov 6,2014. Compared Id of TC_AvalilableRSAPackages instead of PackageId.
		BEGIN
			INSERT INTO TC_SoldRSAPackages	(Name,	MobileNo,	Email,	BranchId,	UserId,	 Quantity,	TC_AvailableRSAPackagesId,	MakeYear,  VersionId, RegistrationNo)
			VALUES							(@Name,	@MobileNo,	@Email, @BranchId,	@UserId, @Quantity,	@PackageId,					@MakeYear, @VersionId, @RegistrationNo)

			SET @Id = SCOPE_IDENTITY()

			INSERT INTO Absure_RSAPolicy(SoldRSAPackagesId) VALUES (@Id)

			UPDATE	TC_AvailableRSAPackages 
			SET		AvailableQuantity = AvailableQuantity-1 , UpdatedOn = GETDATE()
			WHERE	Id = @PackageId --Modified By: Ashwini Dhamankar on Nov 6,2014. Compared Id of TC_AvalilableRSAPackages instead of PackageId.
					AND BranchId=@BranchId
		END
	END
	ELSE  --Individual Type
	BEGIN			
			INSERT INTO TC_SoldRSAPackages (Name, MobileNo, Email, Address, BranchId, UserId, Quantity, MakeYear, VersionId, RegistrationNo, RegistrationType, 
											Kilometers, ChassisNo, EngineNo,CarFittedWith, CityId, AreaId, Amount, ReqRSAPackageId)
			VALUES(@Name, @MobileNo, @Email, @Address, @BranchId, @UserId, @Quantity, @MakeYear, @VersionId, @RegistrationNo, @RegistrationType, 
											@Kilometer,	@ChassisNo, @EngineNo, @CarFittedWith, @CityId, @AreaId, @Amount, @ReqRSAPackageId)
			SET @Id = SCOPE_IDENTITY()

			INSERT INTO Absure_RSAPolicy(SoldRSAPackagesId) VALUES (@Id)
	END
	/*start - Modified by   :	Ruchira Patil on 20th Mar 2015 (To update policy no)*/
	SET @PolicyNo = [dbo].[Absure_GenerateRSAPolicyNo](@Id,@IsDealer)
	
	UPDATE TC_SoldRSAPackages SET PolicyNo = @PolicyNo WHERE Id = @Id
	/*end*/			 
END