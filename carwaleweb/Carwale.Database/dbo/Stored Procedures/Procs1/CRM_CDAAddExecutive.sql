IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_CDAAddExecutive]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_CDAAddExecutive]
GO

	-- =============================================
-- Author      : Chetan Navin
-- Create date : 23 June 2014
-- Description : To Add Executives,Update in CRM_CarDealerAssignment and for executive allocation log.
-- Module      : Dealer Panel
-- =============================================
-- EXEC CRM_CDAAddExecutive 1,2,'chetan',NULL,NULL,2
CREATE PROCEDURE [dbo].[CRM_CDAAddExecutive] 
	-- Add the parameters for the stored procedure here
	@Type             TINYINT,
	@OrgId            BIGINT ,
	@ExecName         VARCHAR(50),
	@ExecContact      VARCHAR(15),
	@ExecEmail		  VARCHAR(100),
	@ExecDesignation  VARCHAR(50),
	@SalesExecutiveId BIGINT,
	@CbdId            BIGINT,
	@LeadId           BIGINT,
	@Id               BIGINT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;
	--Inserts Executive details
	IF(@Type = 1)
	BEGIN
		IF(@SalesExecutiveId IS NULL)
		-- Insert statements for procedure here
			INSERT INTO CRM_CDA_DealerSalesExecutive (OrgId,Name,Contact,Email,Designation) VALUES(@OrgId,@ExecName,@ExecContact,@ExecEmail,@ExecDesignation) 
		ELSE
			UPDATE  CRM_CDA_DealerSalesExecutive SET Name = @ExecName , Contact = @ExecContact , @ExecDesignation = @ExecDesignation
			WHERE Id = @SalesExecutiveId

		SET @Id = SCOPE_IDENTITY()
	END
	--Updates CRM_CarDealerAssignment
	IF(@Type = 2)
	BEGIN
		UPDATE CRM_CarDealerAssignment SET SalesExecutiveId = @SalesExecutiveId
		WHERE CBDId = @CbdId

	-- Executive Allocation Log
		INSERT INTO CRM_ExecAllocationLog (ExecId,LeadId,CBDId,DealerId) VALUES(@SalesExecutiveId,@LeadId,@CbdId,@OrgId) 
	END
END

 