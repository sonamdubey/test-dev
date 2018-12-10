IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_InvoiceInsertUPdate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_InvoiceInsertUPdate]
GO

	CREATE Procedure [dbo].[CRM_InvoiceInsertUPdate]    
(    
@InvNumber AS VARCHAR(50)=null,    
@InvDate AS datetime,  
@UpdatedBy AS VARCHAR(100)=null, 
@CreatedOn AS DATETIME, 
@IsActive AS BIT,  
@id AS INT=0,  
@opr AS INT  
)    
as    
begin    
    
if(@opr=1)    
	BEGIN
		INSERT INTO CRM_INVOICES ( Inv_No , Inv_Date , CreatedOn , UpdatedBy , IsActive)
		VALUES ( @InvNumber , @InvDate , @CreatedOn , @UpdatedBy , @IsActive)    
	END
if(@opr=2)    
	BEGIN
	   UPDATE CRM_INVOICES Set Inv_No=@InvNumber,Inv_Date=@InvDate,UpdatedOn=@CreatedOn, IsActive = @IsActive 
	   WHERE ID=@id    
	END
if(@opr=3)    
 Select * from crm_invoices order by Inv_No     
end  
