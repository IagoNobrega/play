@regression
Feature: Login
    Eu como cliente
    Quero fazer login na aplicação
    Para fazer um pedido de compra

  @smoke
  Scenario: Login com campo e-mail vazio
    Given I am on Login screen
    When I click on Login
    Then I see the message "E-mail inválido."

  @regression
  Scenario: Login com campo senha vazio
    Given I am on Login screen
    And I fill e-mail
    When I click on Login
    Then I see the message "Senha inválida."

  @smoke
  @regression
  Scenario: Login com sucesso
    Given I am on Login screen
    And I fill my credentials
    When I click on Login
    Then I am redirected to the home page
