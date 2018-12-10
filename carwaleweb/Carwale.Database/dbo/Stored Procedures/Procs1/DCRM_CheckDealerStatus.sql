IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_CheckDealerStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_CheckDealerStatus]
GO

	-- Autho : 1.Sachin Bharti(20-Feb-2013)
--	     : Call [DCRM_CheckDealerStatus] to check authentication of user
--	     : for any Dealer , what its status to redirect the respone on one
--       : of the pages ServiceDashBoard,DealerSalesStatus or on Blank page
-- =============================================

CREATE PROCEDURE [dbo].[DCRM_CheckDealerStatus]
@DealerId AS INT,
@UserId AS INT,
@Result   INT OUTPUT
AS
BEGIN
	DECLARE @DealerStatus AS TINYINT
	DECLARE @SalesDealerId AS NUMERIC  
	DECLARE @UserRoleID AS TINYINT
	DECLARE @DealerType AS TINYINT
	DECLARE @LeadStatus AS TINYINT
	
	--Create table type variable to store all values on Dealer and User basis 
	DECLARE @UsersRole AS TABLE (ID TINYINT IDENTITY(1,1),RoleID TINYINT,SalesDealerId BIGINT,DealerStatus TINYINT,DealerType TINYINT,LeadStatus TINYINT)
	DECLARE @TotalLoopCount AS TINYINT	--total loop count variable
	DECLARE @LoopControl AS TINYINT=1	--variable used to control loop
	SET @Result = 3
	
	--storing data into table type variable 
	INSERT INTO @UsersRole (RoleID,SalesDealerId,DealerStatus,DealerType,LeadStatus)
		SELECT   DAU.RoleId,ISNULL(DSD.Id, 0)AS DealerID,D.Status,ISNULL(DSD.DealerType,0)AS DealerType,DSD.LeadStatus
		FROM Dealers D WITH (NOLOCK) 
		INNER JOIN DCRM_ADM_UserDealers DAU ON DAU.DealerId = D.ID 
		LEFT JOIN DCRM_SalesDealer DSD WITH (NOLOCK)ON D.ID = DSD.DealerId 
		WHERE ISNULL(IsDealerDeleted,0) <> 1 AND D.ID =@DealerId AND DAU.UserId = @UserId ORDER BY RoleID ASC
	
	SELECT  @TotalLoopCount=COUNT(*) from @UsersRole
	
	IF @TotalLoopCount = 1
		BEGIN
			SELECT @DealerStatus = DealerStatus, @SalesDealerId = SalesDealerId,@UserRoleID =RoleID,@DealerType =DealerType,@LeadStatus=LeadStatus
			FROM @UsersRole
			IF @UserRoleID = 3 AND @SalesDealerId > 0 AND @DealerType = 1  AND @LeadStatus = 1--When user belongs to Sales Field and completely New Dealer and active
				BEGIN
					SET @Result = 1 -- Open for sales -> Send it to Sales
				END 
			ELSE IF @UserRoleID = 4 AND @SalesDealerId > 0 AND @DealerType > 1 -- When user belongs to Back office and open Sales status and not New dealer
				BEGIN
					SET @Result = 1 -- Open for sales -> Send it to Sales
				END
			ELSE IF @UserRoleID = 4  -- When user belongs to Back office and active
				BEGIN
					SET @Result = 2 --Send it to Service
				END
			ELSE IF @UserRoleID = 5  AND @SalesDealerId > 0 AND @DealerType > 1 --When user belongs to Service field and open Sales status and not New dealer
				BEGIN
					SET @Result = 1 -- Open for sales -> Send it to Sales
				END
			ELSE IF @UserRoleID = 5  --When user belongs to Service field 
				BEGIN
					SET @Result = 2 --Send it to Service
				END
			ELSE IF @DealerStatus = 1 AND @SalesDealerId = 0 -- When dealer is inactive and not yet open for sales
				BEGIN
					SET @Result = 3 --will stay on the same page with message
				END
			PRINT @Result
		END
	ELSE IF @TotalLoopCount > 1 --if user have multiple roles
		BEGIN
			SELECT @DealerStatus = DealerStatus, @SalesDealerId = SalesDealerId,@UserRoleID =RoleID,@DealerType =DealerType,@LeadStatus=LeadStatus
			FROM @UsersRole
			IF @SalesDealerId > 0 --if Sales status is open 
				Begin
					SET @Result = 1 --Send it to Sales
				END
			ELSE
				Begin
					SET @Result = 2 --Send it to Service
				END 
		END
 END




