import { Component, ContextType } from 'react';
import { Collapse, Dropdown, DropdownItem, DropdownMenu, DropdownToggle, Navbar, NavbarBrand, NavbarToggler } from 'reactstrap';
import { Link } from 'react-router-dom';
import AuthContext from '../auth/AuthContext';


// TODO navbar when NavbarToggle - page is small (in hamburger menu)
interface NavMenuState {
  collapsed: boolean;
  driverDropOpen: boolean;
  vehicleDropOpen: boolean;
  offenceDropOpen: boolean;
  theftDropOpen: boolean;
}

export class NavMenu extends Component<object, NavMenuState> {
  static displayName = NavMenu.name;
  static contextType = AuthContext;
  declare context: ContextType<typeof AuthContext>;

  constructor(props: object) {
    super(props);

    this.toggleNavbar = this.toggleNavbar.bind(this);
    this.state = {
      collapsed: true,
      driverDropOpen: false,
      vehicleDropOpen: false,
      offenceDropOpen: false,
      theftDropOpen: false,
    };
  }

  toggleNavbar() {
    this.setState({
      collapsed: !this.state.collapsed
    });
  }

  toggleDriverDropdown = () => {
    this.setState((prevState: NavMenuState) => ({
      driverDropOpen: !prevState.driverDropOpen,
    }));
  };

  toggleVehicleDropdown = () => {
    this.setState((prevState: NavMenuState) => ({
      vehicleDropOpen: !prevState.vehicleDropOpen,
    }));
  };

  toggleOffenceDropdown = () => {
    this.setState((prevState: NavMenuState) => ({
      offenceDropOpen: !prevState.offenceDropOpen,
    }));
  };

  toggleTheftDropdown = () => {
    this.setState((prevState: NavMenuState) => ({
      theftDropOpen: !prevState.theftDropOpen,
    }));
  };


  render() {
    if (!this.context) {
      return null;
    }
    const { isLoggedIn, logout } = this.context;
    const driverDropOpen = this.state.driverDropOpen;
    const vehicleDropOpen = this.state.vehicleDropOpen;
    const offenceDropOpen = this.state.offenceDropOpen;
    const theftDropOpen = this.state.theftDropOpen;


    return (
      <header>
        <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" container light>
          <NavbarBrand tag={Link} to="/">TransportRegister</NavbarBrand>
          <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
          {isLoggedIn && (
            <Collapse className="d-sm-inline-flex navbarDropdownBar" isOpen={!this.state.collapsed} navbar>
              {/*<div className="navbarDropdownBar">*/}
              <Dropdown isOpen={driverDropOpen} toggle={this.toggleDriverDropdown} >
                <DropdownToggle caret color="primary">
                  Řidiči
                </DropdownToggle>
                <DropdownMenu>
                  <DropdownItem header>Řidiči</DropdownItem>
                  <DropdownItem tag={Link} to="/driverSearch">Vyhledat</DropdownItem>
                  <DropdownItem tag={Link} to="/driverCreate">Zaregistrovat</DropdownItem>
                  <DropdownItem>?Zobrazit včechny</DropdownItem>
                </DropdownMenu>
              </Dropdown>

              <Dropdown isOpen={vehicleDropOpen} toggle={this.toggleVehicleDropdown}>
                <DropdownToggle caret color="primary">
                  Vozidla
                </DropdownToggle>
                <DropdownMenu>
                  <DropdownItem header>Vozidla</DropdownItem>
                  <DropdownItem tag={Link} to="/vehicleSearch">Vyhledat</DropdownItem>
                  <DropdownItem tag={Link} to="/vehicleCreate">-Úředník - Registrovat</DropdownItem>
                  <DropdownItem>?Zobrazit včechny</DropdownItem>
                </DropdownMenu>
              </Dropdown>

              <Dropdown isOpen={offenceDropOpen} toggle={this.toggleOffenceDropdown}>
                <DropdownToggle caret color="primary">
                  Přestupky
                </DropdownToggle>
                <DropdownMenu>
                  <DropdownItem header>Přestupky</DropdownItem>
                  <DropdownItem tag={Link} to="/offenceCreate">-Policista - registrovat</DropdownItem>
                  <DropdownItem tag={Link} to="/offencePending">-Úředník - Zobrazit nevyřešené (své)</DropdownItem>
                  <DropdownItem>?Vyhledat (asi ne, spíš sobrazit u řidiče TODO)</DropdownItem>
                  <DropdownItem>?Zobrazit včechny</DropdownItem>
                </DropdownMenu>
              </Dropdown>

              <Dropdown isOpen={theftDropOpen} toggle={this.toggleTheftDropdown}>
                <DropdownToggle caret color="primary">
                  Kradená vozidla
                </DropdownToggle>
                <DropdownMenu>
                  <DropdownItem header>Kradená vozidla</DropdownItem>
                  <DropdownItem tag={Link} to="/theftCreate">Evidovat krádež</DropdownItem>
                  <DropdownItem tag={Link} to="/theftFound">-Policista - nahlásit nález</DropdownItem>
                  <DropdownItem>?Vyhledat</DropdownItem>
                  <DropdownItem>?Listina/zobrazit vše (v zadání ano, ale má smysl?)</DropdownItem>
                </DropdownMenu>
              </Dropdown>
              {/*</div>*/}
            </Collapse>
          )}
        </Navbar>
      </header>
    );
  }
}
