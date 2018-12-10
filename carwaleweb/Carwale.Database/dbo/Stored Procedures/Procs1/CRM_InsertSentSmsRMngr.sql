IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_InsertSentSmsRMngr]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_InsertSentSmsRMngr]
GO

	

-- =============================================
-- Author : Jayant Mhatre (01-08-2011)
-- Modifier: Increased Column for inserting Monthly count of Assigned, PQ, TD, Booked
-- Description:	This procedure is used in Automatic Process CRMRManagersSMS Page to insert data in table 
-- =============================================
CREATE PROCEDURE [dbo].[CRM_InsertSentSmsRMngr]
	
	@ManagerId				INT,
    @Name					VARCHAR(150),
    @Mobile					VARCHAR(15),
    @AssignCntMonthly	NUMERIC,
    @AssignCnt				INT,
    @PqReqMonthly			NUMERIC,
    @PqReq					INT,
    @TdReqMonthly			NUMERIC,
    @TdReq					INT,
    @BqCompMonthly			NUMERIC,
    @BqComp					INT
	
	
AS
			
	BEGIN
		SET NOCOUNT ON		
			INSERT INTO CRM_SmsSentMngrs ([Manager Id],Name,[Mobile No],Assign_leadsMonthly,Assign_Leads,PQ_ReqMonthly,PQ_Req,TD_ReqMonthly,TD_Req,BQ_CompMonthly,BQ_Comp,[SMS Send On])
			VALUES (@ManagerId,@Name,@Mobile,@AssignCntMonthly,@AssignCnt,@PqReqMonthly,@PqReq,@TdReqMonthly,@TdReq,@BqCompMonthly,@BqComp,GETDATE())
				
	END

