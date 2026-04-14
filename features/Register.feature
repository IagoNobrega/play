@regression
Feature: Register User
  Eu como cliente
  Quero me cadastrar na aplicacao
  Para fazer um pedido de compra

  Background: Access Register screen
    Given I am on Register screen

  @register
  @smoke
  Scenario: Campo nome vazio
    When I click on Register
    Then I see message "O campo nome deve ser prenchido" on Register

  @register
  @regression
  Scenario: Campo email vazio
    And I fill name
    When I click on Register
    Then I see message "O campo e-mail deve ser prenchido corretamente" on Register

  @register
  @regression
  Scenario: Campo email invalido
    And I fill name
    And I fill e-mail "invalidEmail"
    When I click on Register
    Then I see message "O campo e-mail deve ser prenchido corretamente" on Register

  @register
  @regression
  Scenario: Campo senha vazio
    And I fill name
    And I fill e-mail "iago@teste.com"
    When I click on Register
    Then I see message "O campo senha deve ter pelo menos 6 digitos" on Register

  @register
  @smoke
  @regression
  Scenario: Cadastro com sucesso
    And I fill valid register data
    When I click on Register
    Then I am redirected after register
