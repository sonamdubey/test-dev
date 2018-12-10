IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_InsertUserDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_InsertUserDealers]
GO

	-- =============================================
-- Author:		Vaibhav K
-- Create date: 27-July-2012
-- Description:	Insert a new record for DCRM_ADM_UserDealers
--				Assign a new  user to dealer send
-- Modifier   : 1.Sachin Bharti(25-Jan-2013)
--			  : Call [DCRM].[AssignDealerOnUserRole] to check
--			  : for any User exist to that Dealer if not then insert new entry
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_InsertUserDealers]
	-- Add the parameters for the stored procedure here
	@DealerIds			AS VARCHAR(MAX),
	@UserId				AS NUMERIC,
	@RoleId				AS NUMERIC,
	@UpdatedBy			AS NUMERIC,
	@UpdatedOn			AS DATETIME,
	@Result 			INT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET @Result = -1
	DECLARE @Dealer VARCHAR(50) 
	DECLARE @DealerIndx VARCHAR(50)
	
	IF @DealerIds <> ''
		BEGIN
			SET @DealerIds =  @DealerIds + ',' 	  
				WHILE @DealerIds <> ''
				BEGIN
					SET @DealerIndx = CHARINDEX(',' , @DealerIds)
					IF  @DealerIndx > 0
					   BEGIN 
						  SET @Dealer = LEFT(@DealerIds, @DealerIndx-1)  
						  SET @DealerIds = RIGHT(@DealerIds, LEN(@DealerIds)- @DealerIndx)
			
							EXECUTE [DCRM].[AssignDealerOnUserRole] @UserId,@RoleId,@Dealer,@UpdatedBy	
						END
				END	
		END
	SET @Result = 1
END




